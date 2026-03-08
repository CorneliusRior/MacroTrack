// maybe this isn't the best name for it, I don't know.
using System.Reflection.Metadata.Ecma335;

namespace MacroTrack.Core.DataModels;

public record MacroTotals(
    double Calories,
    double Protein,
    double Carbs,
    double Fat
);

public record MacroTotalsNullable
(
    double? Calories,
    double? Protein,
    double? Carbs,
    double? Fat
);

public record MacroSummary(
    bool NoGoal,
    DateTime From,
    DateTime To,
    string? GoalName,
    MacroTotals Target,
    MacroTotals Actual,
    MacroTotals Remaining,
    MacroTotalsNullable? TargetMin,
    MacroTotalsNullable? TargetMax
);

public enum MacroType
{
    Calories, Protein, Carbs, Fat
}

public record MacroSingleType(
    MacroType Type,
    double Target,
    double Actual,
    double Remaining,
    double? TargetMin,
    double? TargetMax
);

public static class MacroSummaryExtensions
{
    public static MacroSingleType GetSingle(this MacroSummary summary, MacroType type)
    {
        return type switch
        {
            MacroType.Calories => new MacroSingleType(MacroType.Calories, summary.Target.Calories, summary.Actual.Calories, summary.Remaining.Calories, summary.TargetMin?.Calories, summary.TargetMax?.Calories),
            MacroType.Protein => new MacroSingleType(MacroType.Protein, summary.Target.Protein, summary.Actual.Protein, summary.Remaining.Protein, summary.TargetMin?.Protein, summary.TargetMax?.Protein),
            MacroType.Carbs => new MacroSingleType(MacroType.Carbs, summary.Target.Carbs, summary.Actual.Carbs, summary.Remaining.Carbs, summary.TargetMin?.Carbs, summary.TargetMax?.Carbs),
            MacroType.Fat => new MacroSingleType(MacroType.Fat, summary.Target.Fat, summary.Actual.Fat, summary.Remaining.Fat, summary.TargetMin?.Fat, summary.TargetMax?.Fat),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
