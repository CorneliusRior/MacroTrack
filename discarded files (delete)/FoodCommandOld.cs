using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class FoodCommandOld : ICommand
{
    private readonly FoodLogService _service;

    public string Name => "food";
    public IReadOnlyList<string> Aliases => new[] { "f", "foodlog", "foodjournal", "fooddiary" };
    public string Description => "Commands for interfacing with FoodLog.";
    public string Usage => "I haven't decided yet";
    public string LongHelp => @"I haven't decided yet";

    public FoodCommandOld(FoodLogService service)
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
                if (args.Count < 6) return ("Not Enough Arguments, usage: food add [item name] [amount] [calories] [protein] [carbs] [fat] [notes]");

                string itemName = args[1];
                if (!double.TryParse(args[2], out double amount) ||
                    !double.TryParse(args[3], out double calories) ||
                    !double.TryParse(args[4], out double protein) ||
                    !double.TryParse(args[5], out double carbs) ||
                    !double.TryParse(args[6], out double fat))
                {
                    return "Did not parse one or more of the given numbers (amount, calories, protein, carbs, fat).";
                }
                string notes = args.Count > 7 ? args[7] : "";

                try 
                {
                    var entry = _service.AddEntry(DateTime.Now, itemName, amount, calories, protein, carbs, fat, notes);
                    return $"Entry added: {PrintEntry(entry)}";
                }
                catch (Exception ex) {return "Error adding entry: " + ex.Message;}

            case "delete":
                if (args.Count < 2) return ("Not enough arguments, usage: food delete [ID]. 'Type food list' to find ID");

                if (args[1] == "last")
                {
                    try {return $"Deleted last entry: {PrintEntry(_service.DeleteLast())}";}
                    catch (Exception ex) {return "Error deleting last entry: " + ex.Message;}
                }
                
                if (!int.TryParse(args[1], out int id)) return $"Invalid ID '{args[1]}', must be an integer.";
                try {return $"Deleted entry: {PrintEntry(_service.DeleteEntry(id))}";}
                catch (Exception ex) {return "Error deleting entry: " + ex.Message;}
            
            case "list":
                if (args.Count < 2)
                {
                    try {return PrintList(_service.GetAll());}
                    catch (Exception ex) {return "Error getting list: " + ex.Message;}
                }

                if (args.Count == 2)
                {
                    if (!DateTime.TryParse(args[1], out DateTime start)) return $"Cannot parse Date/Time '{args[1]}'.";

                    try {return PrintList(_service.FromTimes(start, DateTime.Now));}
                    catch (Exception ex) {return "Error getting list: " + ex.Message;}
                }

                if (args.Count > 2)
                {
                    if (!DateTime.TryParse(args[1], out DateTime start)) return $"Cannot parse Date/Time '{args[1]}'.";
                    if (!DateTime.TryParse(args[2], out DateTime end)) return $"Cannot parse Date/Time '{args[2]}'.";

                    try {return PrintList(_service.FromTimes(start, end));}
                    catch (Exception ex) {return "Error getting list: " + ex.Message;}
                }

                return "";
            
            default:
                return $"Unknown argument 'food {args[0]}', type 'help food'.";
        }
    }

    private string PrintEntry(FoodEntry entry)
    {
        // Simple readable columns
        return $"{entry.Id,3} {entry.Time:MM/dd/yyyy HH:mm:ss} {entry.ItemName,-15} {entry.Amount,5:0.0} {entry.Calories,7:0} {entry.Protein,7:0.0} {entry.Carbs,7:0.0} {entry.Fat,7:0.0} {entry.Notes}";
    }

    private string PrintList(List<FoodEntry> FoodList)
    {
        if (FoodList == null || FoodList.Count == 0)
            return "No food entries found.";

        string listString = $"ID  Time                ItemName        Amount Calories Protein  Carbs    Fat     Notes\n";
        foreach (var entry in FoodList)
        {
            listString += PrintEntry(entry) + "\n";
        }
        return listString;
    }
}