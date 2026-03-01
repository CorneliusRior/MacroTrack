//GoalActivation is a model for an entry in GoalHistory.

namespace MacroTrack.Core.Models;

public class GoalActivation
{
    // I will need to explain somewhat: When one of these is initially called it only takes the GoalID and activation date. SQL gives it the ID, but activationDate is left as null. Deactivation date is only assigned when another Goal is activated. I'm going to need to rewrite all of this aren't I? ...
    public int Id { get; set; }
    public int GoalId { get; set; }
    public DateOnly ActivatedAt { get; set; }
    public DateOnly DeactivatedAt { get; set; }

    // New
    public GoalActivation(int goalId, DateOnly activatedAt)
    {
        GoalId = goalId;
        ActivatedAt = activatedAt;
        DeactivatedAt = DateOnly.MinValue;
    }


    // Load
    public GoalActivation(int id, int goalId, DateOnly activatedAt)
    {
        Id = id;
        GoalId = goalId;
        ActivatedAt = activatedAt;
        DeactivatedAt = DateOnly.MinValue;
    }

    public GoalActivation(int id, int goalId, DateOnly activatedAt, DateOnly deactivatedAt)
    {
        Id = id;
        GoalId = goalId;
        ActivatedAt = activatedAt;
        DeactivatedAt = deactivatedAt;
    }

    // List (should do this for each of them but we're starting with this one:
    const int PrintIdSpace = -8;
    const int PrintDateSpace = -11;
    public string Print(string dateFormat = "yyyy-MM-dd") => $"#{Id, PrintIdSpace}#{GoalId, PrintIdSpace}{ActivatedAt.ToString(dateFormat), PrintDateSpace}{DeactivatedAt.ToString(dateFormat), PrintDateSpace}";
    public static string PrintHeader() => $"{"ID:",PrintIdSpace} {"Goal ID:",PrintIdSpace} {"Activated:", PrintDateSpace}{"Deactivated:", PrintDateSpace}";
}