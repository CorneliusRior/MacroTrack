namespace MacroTrack.Core.Models;

public class DailyTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool Completed { get; set; }
    public int Streak { get; set; }

    // New 
    public DailyTask(string name, string description)
    {
        Name = name;
        Description = description;
        IsActive = true;
        Completed = false;
    }

    // New short
    public DailyTask(string name)
    {
        Name = name;
        Description = "";
        IsActive = true;
        Completed = false;
    }

    // Load Registry List
    public DailyTask(int id, string name, string description, bool isActive)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    // Load full (before we had streaks: legacy?)
    public DailyTask(int id, string name, string description, bool isActive, bool completed)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        Completed = completed;
    }

    // Load full
    public DailyTask(int id, string name, string description, bool isActive, bool completed, int streak)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        Completed = completed;
        Streak = streak;
    }

    // Print:
    const int PrintCheckSpace = -8;
    const int PrintNameSpace = -20;
    const int PrintDescriptionSpace = -50;
    public static string PrintHeader(bool showIsActive = true, bool showCompleted = true, bool showStreak = false) =>
        $"{"ID:",PrintCheckSpace}" +
        $"{(showIsActive ? $"{"Active:", PrintCheckSpace}" : "")}" +
        $"{(showCompleted ? $"{"Cmplt.:", PrintCheckSpace}" : "")}" +
        $"{(showStreak ? $"{"Streak:", PrintCheckSpace}" : "")}" +
        $"{"Name:", PrintNameSpace}" +
        $"{"Description:", PrintDescriptionSpace}";    
    public string Print(bool showIsActive = true, bool showCompleted = true, bool showStreak = false) =>
        $"{$"#{Id}".Truncate(PrintCheckSpace), PrintCheckSpace}" +
        $"{(showIsActive ? $"{IsActive.Checked(), PrintCheckSpace}" : "")}" +
        $"{(showCompleted ? $"{Completed.Checked(), PrintCheckSpace}" : "")}" +
        $"{(showStreak ? $"{Streak.ToStringTruncate(PrintCheckSpace), PrintCheckSpace}" : "")}" +
        $"{Name.Truncate(PrintNameSpace), PrintNameSpace}" +
        $"{Description.Truncate(PrintDescriptionSpace),PrintDescriptionSpace}";
}