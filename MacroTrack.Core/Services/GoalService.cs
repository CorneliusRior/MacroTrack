namespace  MacroTrack.Core.Services;

using MacroTrack.Core.DataModels;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

/// <summary>
/// Service for interacting with Goal data and repo, add, retrieve, delete goals and goalactivation, find present goal, next goal, and get data.
/// </summary>
/// <remarks>
/// Updated for logging.
/// </remarks>
public class GoalService : ServiceBase
{
    private readonly GoalRepo _repo;

    public GoalService(GoalRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    // New Goal
    public Goal AddGoal(string name, double calories, double protein, double carbs, double fat, GoalType goalType = GoalType.None, string? customType = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar = null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        try 
        {            
            _repo.AddGoal(new Goal(name, calories, protein, carbs, fat, goalType, customType, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat));
            var added = GetGoal(_repo.ReturnLastId());
            Log($"Added goal id #{added.Id}", LogLevel.Info);
            return added;
        }
        catch (Exception ex) 
        {
            Log($"Error adding goal. Incorrect formatting or returned null?", LogLevel.Warning, ex);
            throw;
        }
        
        
    }
    
    // Get Goal from a "GoalActivation"
    public Goal G(GoalActivation activation) // maybe should rename this idk/
    {
        var goal = _repo.GetGoal(activation.GoalId);
        if (goal == null)
        {
            var ex = new Exception("Null return");
            Log($"Error getting goal [{activation.GoalId}], probably doesn't exist.", LogLevel.Warning, ex);
            throw ex;
        }
        return goal;
    }

    // Load 1
    public Goal GetGoal(int id)
    {
        var goal = _repo.GetGoal(id);
        if (goal == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error getting Goal #{id}, goal of this ID probably doesn't exist", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Requested Goal #{id}");
        return goal;
    }

    // Load all (list)
    public List<Goal> GetAllGoals()
    {
        Log();
        return _repo.GetAllGoals();
    }

    // Edit 
    public Goal EditGoal(int id, string name, double calories, double protein, double carbs, double fat, GoalType goalType = GoalType.None, string? customType = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar = null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        Log($"Called on Goal #{id}");
        // Make sure ID still exists, given that it is an "update" command, it doesn't really matter, so we don't really need to have an exception for it, just a warning.
        if (_repo.GetGoal(id) == null) Log($"Warning: No goal with ID '{id}' could be found. No edit will be made.", LogLevel.Warning);
        var goal = new Goal(id, name, calories, protein, carbs, fat, goalType, customType, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
        _repo.EditGoal(id, goal);       
        return GetGoal(id);
    }

    // Delete
    public Goal DeleteGoal(int id)
    {
        Log($"Request delete Goal id #{id}");
        var goal = GetGoal(id);
        _repo.DeleteGoal(id);
        return goal;
    }

    // Activate Goal
    public GoalActivation ActivateGoal(int id, DateOnly? date = null)
    {
        try 
        {
            Goal? goal = _repo.GetGoal(id);
            if (goal == null) throw new Exception($"_repo.GetGoal({id}) returned as null: Goal probably doesn't exist");
            var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
            var goalActivation = new GoalActivation(goal.Id, parsedDate);

            // see if there is an active goal already, if so, set it to deactivate on that date.
            var presentGoal = GetPresentGoal(parsedDate);
            if (presentGoal != null) _repo.Deactivate(presentGoal.Id, parsedDate);
                    
            _repo.ActivateGoal(goalActivation);

            // See if there are any goals set as active after this date, if so, give our new goal activation a deactivation date.
            var nextGoal = _repo.GetNextGoal(parsedDate);
            if (nextGoal != null) _repo.Deactivate(_repo.ReturnLastActivationId(), nextGoal.ActivatedAt);

            var result = _repo.GetPresentGoal(parsedDate);
            if (result == null) { throw new Exception("GoalActivation returned as null"); }
            Log($"Activated Goal #{id} at time '{parsedDate}'");
            return result;
        }
        catch (Exception ex) 
        {
            Log($"Error activating Goal #{id} at '{(date.HasValue ? "Null (now)" : date)}'", LogLevel.Warning, ex);
            throw;
        }
    }

    // Deactivate - Undo activation (delete GoalActivation row)
    public GoalActivation DeleteGoalActivation(int id)
    {
        // Does goal exist?
        var entry = _repo.GetGoalActivation(id);
        if (entry == null)
        {
            var ex = new Exception("Returned as null");
            Log($"Error deleting GoalActivation, entry {id} probably doesn't exist.", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteGoalActivation(id);
        return entry;
    }

    public GoalActivation? GetGoalActivation(int id)
    {
        return _repo.GetGoalActivation(id);
    }

    // Get present active goal
    public GoalActivation? GetPresentGoal(DateOnly? date = null)
    {
        Log();
        var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
        return _repo.GetPresentGoal(parsedDate);
    }

    // Get next goal
    public GoalActivation? GetNextGoal(DateOnly? date = null)
    {
        Log();
        var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
        return _repo.GetNextGoal(parsedDate);
    }

    // Get active goal history
    public List<GoalActivation> GetGoalHistory()
    {
        Log();
        return _repo.GetGoalHistory();
    }

    // Get activate goal history as tuple of activationdate and goal calories, used for WPF calGraph:
    public List<(DateTime date, double cal)> GetTupleGoalHistory(DateTime startTime, DateTime endTime, bool clampStartTime = false)
    {
        // Get the data from repo:
        var history = _repo.GetGoalHistoryInterval(startTime, endTime);
        List<(DateTime date, double cal)> tupleList = new();
        foreach (GoalActivation ga in history)
        {
            DateTime date = ga.ActivatedAt.ToDateTime(TimeOnly.MinValue);
            if (clampStartTime && date < startTime) date = startTime;
            Goal? g = _repo.GetGoal(ga.GoalId);
            if (g is null) continue; // Goal has been deleted presumably.
            tupleList.Add( (date, g.Calories) );
        }
        return tupleList;
    }

    public MacroTotals GetMacroGoals(DateTime startTime, int timeFrame)
    {
        Log($"Requested from '{startTime}' for '{timeFrame}' days");
        // get the end time:
        var endTime = startTime.AddDays(timeFrame);
        var goalHistory = _repo.GetGoalHistoryInterval(startTime, endTime);

        double cal = 0;
        double pro = 0;
        double car = 0;
        double fat = 0;

        
        // see how many days each goal was active for, and add that to the totals.
        DateOnly startDate = DateOnly.FromDateTime(startTime);
        DateOnly endDate = DateOnly.FromDateTime(endTime);
        for (int i = 0; i < goalHistory.Count; i++)
        {
            var ga = goalHistory[i];
            var g = _repo.GetGoal(ga.GoalId);
            if (g == null) continue; // Skip if goal doesn't exist
            
            int daysActive;
            // check there's no valid deactivation date:
            if (ga.DeactivatedAt > endDate || ga.DeactivatedAt == DateOnly.MinValue)
            {
                // check if it was activated before start time
                if (ga.ActivatedAt < startDate) daysActive = endDate.DayNumber - startDate.DayNumber;
                else daysActive = endDate.DayNumber - ga.ActivatedAt.DayNumber;
            }
            else // there is a valid deactivation date!! ^^
            {
                DateOnly effectiveDeactivation = ga.DeactivatedAt > endDate ? endDate : ga.DeactivatedAt;
                if (ga.ActivatedAt < startDate) daysActive = effectiveDeactivation.DayNumber - startDate.DayNumber;
                else daysActive = effectiveDeactivation.DayNumber - ga.ActivatedAt.DayNumber;
            }
            cal += g.Calories * daysActive;
            pro += g.Protein * daysActive;
            car += g.Carbs * daysActive;
            fat += g.Fat * daysActive;
        }

        var totals = new MacroTotals(
            Calories: cal,
            Protein: pro,
            Carbs: car,
            Fat: fat
        );
        return totals;
    }

    public List<GoalActivation> GetActivationsOfGoal(int id)
    {
        return _repo.GetActivationsOfGoal(id);
    }
}