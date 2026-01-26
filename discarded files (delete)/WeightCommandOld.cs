using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class WeightCommandOld : ICommand
{
    private readonly WeightLogService _service;

    public string Name => "weight";
    public IReadOnlyList<string> Aliases => new[] { "w", "weightlog" };
    public string Description => "Commands for interfacing with WeightLog.";
    public string Usage => "weight [add/delete/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with WeightLog. Subcommands are:
     - weight add [weight] (date)            - Adds a weight entry to the WeightLog. Time is logged as now if (time) is left vacant. Use format 'YYYY/MM/DD HH:MM:DD' in double quotes by default, but this can accept almost any format.
     - weight delete [ID]                    - Deletes the selected entry. No (Y/N) confirmation.
     - weight list (start time) (end time)   - lists every item in WeightLog, or every item since start time if present, or every item between start and end time if both arguments present.";

    public WeightCommandOld(WeightLogService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) 
        {
            return ("Not enough arguments, usage: " + Usage);
        }

        switch(args[0])
        {
            case "add":
                if (args.Count == 1)
                {
                    return ("Not enough arguments, usage: weight add [weight] (time)");
                }

                if (!double.TryParse(args[1], out double weight)) 
                {
                    return $"Invalid weight '{args[1]}', must be a number (double).";
                }

                if (args.Count == 2)
                {
                    return PrintEntry(_service.AddEntry(DateTime.Now, weight));
                }
                
                if (args.Count == 3)
                {
                    // this also works with "YYYY/MM/DD HH:MM:SS", all in quotes
                    if (DateTime.TryParse(args[2], out DateTime parsedTime))
                    {
                        return PrintEntry(_service.AddEntry(parsedTime, weight));
                    }
                    else
                    {
                        return $"Invalid date/time '{args[2]}', please provide a valid datetime (e.g., YYYY/MM/DD HH:MM:SS or similar).";
                    }
                }

                return "";
            
            case "delete":
                if (args.Count == 1)
                {
                    return ("Not enough arguments, usage: weight delete [ID]. 'Type weight list' to find ID");
                }

                if (!int.TryParse(args[1], out int id))
                {
                    return $"Invalid ID '{args[1]}', must be an integer";
                }
                
                try
                {
                    var DeletedEntry = _service.DeleteEntry(id);
                    return $"Deleted Entry: {PrintEntry(DeletedEntry)}";
                }
                catch
                {
                    return "ID not found";
                }
                

            case "list":
                if (args.Count < 2)
                {
                    return PrintList(_service.GetAll());
                }
                
                if (args.Count == 2)
                {
                    if (!DateTime.TryParse(args[1], out DateTime startTime)) return $"Cannot parse Date/Time '{args[1]}'.";
                    return PrintList(_service.FromTimes(startTime, DateTime.Now));
                }
                else {
                    if (!DateTime.TryParse(args[1], out DateTime startTime)) return $"Cannot parse Date/Time '{args[1]}'.";
                    if (!DateTime.TryParse(args[2], out DateTime endTime)) return $"Cannot parse Date/Time '{args[2]}'.";
                    return PrintList(_service.FromTimes(startTime, endTime));
                }
                       
            default:
                return $"Unknown argument 'weight {args[0]}', type 'help weight'.";
        }
    }

    private string PrintEntry(WeightEntry entry)
    {
        return $"{$"[{entry.Id}]",5} {entry.Weight,6:F1}Kg {entry.Time}";
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