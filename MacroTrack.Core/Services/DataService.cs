// making a service to have only one command give you a load of data. I kind of wonder what the point of all the methods in the other services will be when this exists, but oh well.
namespace MacroTrack.Core.Services;

using MacroTrack.Core.Repositories;
using MacroTrack.Core.Models;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;

using System.Runtime.CompilerServices;
using MacroTrack.Core.Infrastructure;

/// <summary>
/// General service with access to serveral Repos, used to get data for various purposes.
/// Has no write ability.
/// </summary>
public class DataService : ServiceBase
{
    FoodLogRepo _foodLogRepo;
    GoalRepo _goalRepo;
    WeightLogRepo _weightLogRepo;


    public DataService(FoodLogRepo foodLogRepo, GoalRepo goalRepo, WeightLogRepo weightLogRepo, CoreContext ctx) : base(ctx)
    {
        // RepoList here.
        _foodLogRepo = foodLogRepo;
        _goalRepo = goalRepo;
        _weightLogRepo = weightLogRepo;
    }

    // MacroSummary:
    public MacroSummary GetMacroSummary(DateTime startTime, DateTime endTime)
    {
        Log($"Generating MacroSummary for period: {startTime} to {endTime}");
        MacroTotals target = GetTarget(startTime, endTime);
        MacroTotals actual = GetActual(startTime, endTime);
        MacroTotals remaining = GetRemaining(target, actual);
        MacroTotalsNullable? Min = null;
        MacroTotalsNullable? Max = null;
        if ((endTime - startTime).Days == 1)
        {
            Log("Determined that it is a single day, getting MinMax", LogLevel.Info);
            Min = GetMinMax(startTime, false);
            Max = GetMinMax(startTime, true);
        }
        else Log("Detemined that is is not a single day, no MinMax", LogLevel.Info);
        var presentGoal = _goalRepo.GetPresentGoal(DateOnly.FromDateTime(startTime));
        var goalName = presentGoal != null ? _goalRepo.GetGoal(presentGoal.GoalId)?.GoalName ?? "No Goal" : "No Goal";
        return new MacroSummary(        
            NoGoal: presentGoal == null ? true : false,
            From: startTime,
            To: endTime,
            GoalName: goalName,
            Target: target,
            Actual: actual,
            Remaining: remaining,
            TargetMin: Min,
            TargetMax: Max            
        );
    }

    public MacroTotals GetTarget(DateTime startTime, DateTime endTime)
    {
        // get the end time:
        var goalHistory = _goalRepo.GetGoalHistoryInterval(startTime, endTime);
        Log($"{goalHistory.Count} goals counted during this period.");
        
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
            var g = _goalRepo.GetGoal(ga.GoalId);
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

    public MacroTotalsNullable GetMinMax(DateTime time, bool max)
    {
        // Min/Max should only be given if the time period is one day (or less), if we try adding things over a longer period of time with different goals, we end up needing to add up nulls, and it gets very messy really quickly, so we'll just stick to this. Using longer time periods isn't much of a primary feature anyway, I barely ever do it.
        Log();
        DateOnly date = DateOnly.FromDateTime(time);
        GoalActivation? ga = _goalRepo.GetPresentGoal(date);
        if (ga == null)
        {
            Log("Determined ga as null");
            return new MacroTotalsNullable(null, null, null, null);
        }
        Goal? g = _goalRepo.GetGoal(ga.GoalId);
        if (g == null)
        {
            Log("Determined g as null");
            return new MacroTotalsNullable(null, null, null, null); // Hopefully this should never happen
        }
        if (max)
        {
            Log("Determined as max, returning new MacroTotalsNullable");
            return new MacroTotalsNullable(
                Calories: g.MaxCal,
                Protein: g.MaxPro,
                Carbs: g.MaxCar,
                Fat: g.MaxFat
            );
        }
        Log("Determined as min, returning new MacroTotalsNullable");
        return new MacroTotalsNullable(
            Calories: g.MinCal,
            Protein: g.MinPro,
            Carbs: g.MinCar,
            Fat: g.MinFat
        );

    }

    public MacroTotals GetActual(DateTime startTime, DateTime endTime)
    {
        var foodLog = _foodLogRepo.FromTimes(startTime, endTime);
        double calories = 0;
        double protein = 0;
        double carbs = 0;
        double fat = 0;
        foreach (var food in foodLog)
        {
            calories += food.Calories;
            protein += food.Protein;
            carbs += food.Carbs;
            fat += food.Fat;
        }
        return new MacroTotals(        
            Calories: calories,
            Protein: protein,
            Carbs: carbs,
            Fat: fat
        );
    }

    public MacroTotals GetRemaining(MacroTotals target, MacroTotals actual)
    {
        return new MacroTotals(
            Calories: target.Calories - actual.Calories,
            Protein: target.Protein - actual.Protein,
            Carbs: target.Carbs - actual.Carbs,
            Fat: target.Fat - actual.Fat
        );
    }

    // Weight:
    public List<WeightEntry> GetWeightEntries(DateTime startTime, DateTime endTime)
    {
        Log($"Fetching WeightEntry data from period: {startTime} to {endTime}");
        return _weightLogRepo.FromTimes(startTime, endTime); // identical to the one in WeightLogService really, but feels like good practice to have it here.
    }

    // Calorie intake:

    // Get Calorie series
    public List<CalSeriesPoint> GetCalSeries(DateTime startTime, DateTime endTime)
    {
        Log($"Fetching Calories series for period: {startTime} to {endTime}");
        endTime = endTime.AddDays(1); // So that we can get all the entries until midnight.
        List<(DateTime, double)> CalSeriesList = _foodLogRepo.DailySumRange("Calories", startTime, endTime);
        List<CalSeriesPoint> CalSeries = new List<CalSeriesPoint>();

        int i = 0;

        foreach ((DateTime, double) t in CalSeriesList) // t for tuple
        {
            CalSeriesPoint point = new CalSeriesPoint
            {
                Id = i++,
                Date = t.Item1,
                Calories = t.Item2
            };
            CalSeries.Add(point);
        }


        return CalSeries;
    }

    // Get Goal series:
    public List<GoalSeriesPoint> GetGoalSeries(DateTime startTime, DateTime endTime)
    {
        List<GoalActivation> GoalHistory = _goalRepo.GetGoalHistoryInterval(startTime, endTime);
        List<GoalSeriesPoint> GoalSeries = new List<GoalSeriesPoint>();

        string NoGoalString = ($"Core.Services.DataService.GetGoalSeries(): Error loading first entry: corresponding goal does not exist.");

        // Take the first entry, if GoalActivation start date proceeds startDate, that is taken as "no Goal", so we will have no point there, line starts on that date. Otherwise, set a point one day before startDate

        int i = 1; // we are declaring this before we do "first", so that we can increase it if we find deactivated at.
        GoalActivation first = GoalHistory[0];
        double cal = _goalRepo.GetCalories(first.GoalId);
        if (cal < 0) { throw new Exception(NoGoalString); }
        else
        {
            if (DateOnly.FromDateTime(startTime).CompareTo(first.ActivatedAt) < 0)
            {
                GoalSeriesPoint point = new GoalSeriesPoint
                {
                    Id = 0,
                    Date = first.ActivatedAt.ToDateTime(TimeOnly.MinValue),
                    Calories = cal
                };
                GoalSeries.Add(point);
            }
            else
            {
                GoalSeriesPoint point = new GoalSeriesPoint
                {
                    Id = 0,
                    Date = startTime.AddDays(-1),
                    Calories = cal
                };
                GoalSeries.Add(point);
            }            

            if (first.DeactivatedAt != DateOnly.MinValue && first.DeactivatedAt.CompareTo(DateOnly.FromDateTime(endTime)) < 0)
            {
                GoalSeriesPoint point = new GoalSeriesPoint
                {
                    Id = i++,
                    Date = first.DeactivatedAt.ToDateTime(TimeOnly.MinValue),
                    Calories = cal
                };
                GoalSeries.Add(point);
            }
        }

        // If there's only one, then add another point today and return it:
        if (GoalHistory.Count == 1)
        {
            GoalSeriesPoint Point = new GoalSeriesPoint
            {
                Id = i,
                Date = endTime,
                Calories = cal
            };
            GoalSeries.Add(Point);
            return GoalSeries;
        }

        // Otherwise, remove the first in the list, iterate through them all:
        GoalHistory.Remove(GoalHistory[0]); // The IDs shift when you do that right?
        foreach (GoalActivation a in GoalHistory)
        {
            GoalSeriesPoint aPoint = new GoalSeriesPoint
            {
                Id = i++,
                Date = a.ActivatedAt.ToDateTime(TimeOnly.MinValue),
                Calories = _goalRepo.GetCalories(a.GoalId)
            };
            if (aPoint.Calories < 0) throw new Exception(NoGoalString);
            else GoalSeries.Add(aPoint);

            if (a.DeactivatedAt != DateOnly.MinValue && a.DeactivatedAt.CompareTo(DateOnly.FromDateTime(endTime)) <= 0)
            {
                GoalSeriesPoint dPoint = new GoalSeriesPoint
                {
                    Id = i++,
                    Date = a.DeactivatedAt.ToDateTime(TimeOnly.MinValue),
                    Calories = _goalRepo.GetCalories(a.GoalId)
                };
                if (dPoint.Calories >= 0) GoalSeries.Add(dPoint);
            }
        }

        // Finally, add a point right at the end equal to the last one
        GoalSeriesPoint last = new GoalSeriesPoint
        {
            Id = GoalSeries.Last().Id + 1,
            Date = endTime,
            Calories = GoalSeries.Last().Calories
        };
        GoalSeries.Add(last);
        return GoalSeries;
    }
}