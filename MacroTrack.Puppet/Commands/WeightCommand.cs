using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class WeightCommand : ICommand
{
    private readonly WeightLogService _service;

    public string Name => "weight";
    public IReadOnlyList<string> Aliases => new[] { "w", "weightlog" };
    public string Description => "Commands for interfacing with WeightLog.";
    public string Usage => "weight [add/delete/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with WeightLog. Subcommands are:
     - weight add [weight] (date)            - Adds a weight entry to the WeightLog. Time is logged as now if (time) is left vacant. Use format 'YYYY/MM/DD HH:MM:DD' in double quotes by default, but this can accept almost any format.
     - weight delete [ID]/last               - Deletes the selected entry. No (Y/N) confirmation. typing 'last' instead of ID will delete the last entry (entry with highest ID).
     - weight list (start time) (end time)   - lists every item in WeightLog, or every item since start time if present, or every item between start and end time if both arguments present.";

    public WeightCommand(WeightLogService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return ("Not enough arguments, usage: " + Usage);
        switch (args[0])
        {
            case "add": return Add(args.Skip(1).ToList());
            case "delete": return Delete(args.Skip(1).ToList());
            case "list": return List(args.Skip(1).ToList());
            case "help": return LongHelp;
            default: return $"Unknown argument 'weight {args[0]}', type 'help weight'.";
        }
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: weight add [weight] (date)";
        if (!double.TryParse(args[0], out double weight)) return $"Invalid weight '{args[0]}', must be a number (double)";
        if (args.Count == 1)
        {
            try {return PrintEntry(_service.AddEntry(DateTime.Now, weight));}
            catch (Exception ex) {return $"Puppet.Commands.WeightCommand.Add(): Error adding weight: {ex}";}
        }
        if (!DateTime.TryParse(args[1], out DateTime time)) return $"Invalid date '{args[1]}', \nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\".";
        try {return PrintEntry(_service.AddEntry(time, weight));}
        catch (Exception ex) {return $"Puppet.Commands.WeightCommand.Add(): Error adding weight and time: {ex}";}
    }

    private string Delete(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: weight delete [ID]/last. Type 'weight list' to find ID";
        if (args[0] == "last")
        {
            try {return $"Deleted last entry: {PrintEntry(_service.DeleteLast())}";}
            catch (Exception ex) {return $"Puppet.Commands.WeightCommand.Delete(): Error deleting last entry: {ex.Message}";}
        }
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        try {return $"Deleted entry: {PrintEntry(_service.DeleteEntry(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.WeightCommand.Delete(): Error deleting entry: {ex.Message}";}
    }

    private string List(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return PrintList(_service.GetAll());
        if (!DateTime.TryParse(args[0], out DateTime start)) return $"Cannot parse Date/Time '{args[0]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        if (args.Count == 1) return PrintList(_service.FromTimes(start, DateTime.Now));
        if (!DateTime.TryParse(args[1], out DateTime end)) return $"Cannot parse Date/Time '{args[1]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        return PrintList(_service.FromTimes(start, end));
    }

    private string PrintEntry(WeightEntry entry)
    {
        return $"{$"[{entry.Id}]",5} {entry.Weight,6:F1}Kg {entry.Time:yyyy/MM/dd HH:mm:ss}";
    }

    private string PrintList(List<WeightEntry> WeightList)
    {
        string listString = "";
        foreach (var entry in WeightList)
        {
            listString = $"{listString}{PrintEntry(entry)}\n";
        }
        return listString;
    }
}