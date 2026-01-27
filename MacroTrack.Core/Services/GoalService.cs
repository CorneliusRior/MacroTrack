namespace  MacroTrack.Core.Services;

using MacroTrack.Core.AppModels;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class GoalService : ServiceBase
{
    private readonly GoalRepo _repo;

    public GoalService(GoalRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    // New Goal
    public Goal AddGoal(string name, double calories, double protein, double carbs, double fat, string? type = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar = null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        try 
        {
            var goal = new Goal(name, calories, protein, carbs, fat, notes, type, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
            _repo.AddGoal(goal);
            return GetGoal(_repo.ReturnLastId());
        }
        catch (Exception ex) {throw new Exception($"Core.Services.GoalService.AddGoal(): Error creating goal: {ex.Message}");}
        
        
    }
    
    // Get Goal from a "GoalActivation"
    public Goal G(GoalActivation activation) // maybe should rename this idk/
    {
        return _repo.GetGoal(activation.GoalId);
    }

    // Load 1
    public Goal GetGoal(int id)
    {
        var goal = _repo.GetGoal(id);
        if (goal == null) throw new Exception($"Core.Services.GoalService.GetGoal(): Error getting goal, returned as null. Goal probably doesn't exist, or wrong ID: [{id}].");
        return goal;
    }

    // Load all (list)
    public List<Goal> GetAllGoals()
    {
        var goals = _repo.GetAllGoals();
        if (goals == null) throw new Exception("Core.Services.GoalService.GetAllGoals(): Error getting goals, returned as null. No goals found.");
        return goals;
    }

    // Edit 
    public Goal EditGoal(int id, string name, double calories, double protein, double carbs, double fat, string? type = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar = null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        var goal = new Goal(id, name, calories, protein, carbs, fat, notes, type, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
        _repo.EditGoal(id, goal);
        return GetGoal(id);
    }

    // Delete
    public Goal DeleteGoal(int id)
    {
        var goal = GetGoal(id);
        _repo.DeleteGoal(id);
        return goal;
    }

    // Activate Goal
    public GoalActivation ActivateGoal(int id, DateOnly? date = null)
    {
        try 
        {
            var goal = GetGoal(id);
            var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
            var goalActivation = new GoalActivation(goal.Id, parsedDate);

            // see if there is an active goal already, if so, set it to deactivate on that date.
            var presentGoal = GetPresentGoal(parsedDate);
            if (presentGoal != null)
            {
                _repo.Deactivate(presentGoal.Id, parsedDate);
            }
                    
            _repo.ActivateGoal(goalActivation);

            // See if there are any goals set as active after this date, if so, give our new goal activation a deactivation date.
            var nextGoal = _repo.GetNextGoal(parsedDate);
            if (nextGoal != null)
            {
                _repo.Deactivate(_repo.ReturnLastActivationId(), nextGoal.ActivatedAt);
            }

            var result = _repo.GetPresentGoal(parsedDate);
            if (result == null) throw new Exception("Core.Services.GoalService.ActivateGoal(): Error retrieving activated goal after insertion.");
            return result;
        }
        catch (Exception ex) {throw new Exception($"Core.Services.GoalService.ActivateGoal(): Error getting GoalActivation, probably wrong ID: {ex.Message}");}
    }

    // Ensure Deactivation entries: This is mostly a temporary function to fix the current "debug" database, hopefully we won't ever actually need it.
    public void EnsureDeactivationEntries()
    {
        var goalHistory = _repo.GetGoalHistory();
        for (int i = 0; i < goalHistory.Count - 1; i++)
        {
            var ga = goalHistory[i];
            if (i != goalHistory.Count - 1)
            {
                var deactivationDate = goalHistory[i + 1].ActivatedAt;
                _repo.Deactivate(ga.Id, deactivationDate);
            }
        }
    }

    // Deactivate - Undo activation (delete GoalActivation row)
    public GoalActivation DeleteGoalActivation(int id)
    {
        // Does goal exist?
        var entry = GetGoalActivation(id);
        if (entry == null) throw new Exception($"Core.Services.GoalService.DeleteGoalActivation(): Error getting GoalActivation, probably doesn't exist.");
        _repo.DeleteGoalActivation(id);
        return entry;
    }

    public GoalActivation? GetGoalActivation(int id)
    {
        return _repo.GetGoalActivation(id);
        // if it returns as null, then it doesn't exist (probably)
    }

    // Get present active goal
    public GoalActivation? GetPresentGoal(DateOnly? date = null)
    {
        var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
        return _repo.GetPresentGoal(parsedDate);
    }

    // Get next goal
    public GoalActivation? GetNextGoal(DateOnly? date = null)
    {
        var parsedDate = date ?? DateOnly.FromDateTime(DateTime.Now);
        return _repo.GetNextGoal(parsedDate);
    }

    // Get active goal history
    public List<GoalActivation> GetGoalHistory()
    {
        return _repo.GetGoalHistory();
    }

    public MacroTotals GetMacroGoals(DateTime startTime, int timeFrame)
    {
        // get the end time:
        var endTime = startTime.AddDays(timeFrame);
        var goalHistory = _repo.GetGoalHistoryInterval(startTime, endTime);
        var totals = new MacroTotals
        {
            Calories = 0,
            Protein = 0,
            Carbs = 0,
            Fat = 0
        };
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
            totals.Calories += g.Calories * daysActive;
            totals.Protein += g.Protein * daysActive;
            totals.Carbs += g.Carbs * daysActive;
            totals.Fat += g.Fat * daysActive;
        }
        return totals;
    }
}