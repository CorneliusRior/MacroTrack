// maybe this isn't the best name for it, I don't know.
namespace MacroTrack.Core.AppModels;

public record MacroTotals(
    double Calories,
    double Protein,
    double Carbs,
    double Fat
);

public record MacroSummary(
    bool NoGoal,
    DateTime From,
    DateTime To,
    string? GoalName,
    MacroTotals Target,
    MacroTotals Actual,
    MacroTotals Remaining
);