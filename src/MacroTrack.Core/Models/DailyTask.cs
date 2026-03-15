using System.Text;

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

    // Clone:
    public DailyTask Clone() => new DailyTask(Id, Name, Description, IsActive, Completed, Streak);    

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
    public string PrintRef() => $"#{Id} `{Name}`";
}

// History classes:
public sealed class TaskHistoryRow
{
    public DateTime Date { get; init; }
    public Dictionary<int, bool> Completed { get; init; } = new();

    public TaskHistoryRow(DateTime date, Dictionary <int, bool> completed)
    {
        Date = date;
        Completed = completed;
    }

    public string PrintRow()
    {
        StringBuilder sb = new();
        sb.Append(Date.ToString("yyyy-MM-dd |"));
        foreach (var task in Completed) sb.Append($" {task.Value.Checked()} |");
        return sb.ToString();
    }
}

public sealed class TaskHistoryMatrix
{
    public List<DailyTask> Tasks { get; init; } = new();
    public List<TaskHistoryRow> Rows { get; init; } = new();

    public TaskHistoryMatrix(Dictionary<DateTime, Dictionary<int, bool>> hist, List<DailyTask> registry)
    {
        List<TaskHistoryRow> rows = new();
        foreach (var row in hist) rows.Add(new TaskHistoryRow(row.Key, row.Value));
        Tasks = registry;
        Rows = rows;
    }

    public string Print(int max = 300)
    {
        StringBuilder sb = new();
        sb.AppendLine("Printing History\n");
        sb.AppendLine("Task index:");
        foreach (DailyTask task in Tasks) sb.AppendLine(task.PrintRef());
        sb.AppendLine("");
        sb.Append("           |");
        foreach (DailyTask task in Tasks) sb.Append(task.Id.ToStringTruncate(5, "#").PadRight(5) + '|');
        sb.AppendLine("");
        foreach (TaskHistoryRow row in Rows) sb.AppendLine(row.PrintRow().Truncate(max));
        return sb.ToString();
    }
}