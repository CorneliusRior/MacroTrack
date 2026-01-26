// maybe this isn't the best name for it, I don't know.
namespace MacroTrack.Core.AppModels;

public record MacroTotals
{
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
}

public record MacroSummary
{
    public bool NoGoal { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? GoalName { get; set; }
    public MacroTotals Target { get; set; }
    public MacroTotals Actual { get; set; }
    public MacroTotals Remaining { get; set; }
}