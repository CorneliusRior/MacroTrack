// rewritten

using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class FoodCommand : ICommand
{
    private readonly FoodLogService _service;
    private readonly PresetService _presetService;

    public string Name => "food";
    public IReadOnlyList<string> Aliases => new[] { "f", "foodlog", "foodjournal", "fooddiary" };
    public string Description => "Commands for interfacing with FoodLog.";
    public string Usage => "food [add/delete/edit/list/sum] (other arguments).";
    public string LongHelp => @"Command set for interfacing with food log. Subcommands are:
     - food add [item name] [amount] [calories] [protein] [carbs] [fat] (category) (notes) 
                                                - adds an entry into FoodLog. This is a very long and cumbersome way of doing it, frankly you're not supposed to type it all out, UI elements should make this a much quicker action, this is just here for debugging &c.
     - food delete [ID]/last                    - Deletes the selected entry, or the last one if 'last' is written in it's place. Deletes immediately with no (Y/N) confirmation.
     - food edit [ID] [item name] [amount] [calories] [protein] [carbs] [fat] (category) (notes) 
                                                - Edits entry (unimplemented)
     - food list (start time) (end time)        - Returns entries. Given no arguments, returns all. Given (start time), returns all entries from start time until now. Given both (start time) and (end time), returns all entries in that period.
     - food listcat [category]                  - Lists all entries with the specified category.
     - food preset [ID] (multiplier) (notes)    - Adds a food entry based on the ID, multiplied by the multiplier, and notes added.
     - food sum [tbh]                           - returns sum of parameters for a period, very important.";

    public FoodCommand(FoodLogService service, PresetService presetService)
    {
        _service = service;
        _presetService = presetService;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return ("Not enough arguments, usage: " + Usage);
        switch (args[0])
        {
            case "add": return Add(args.Skip(1).ToList());
            case "delete": return Delete(args.Skip(1).ToList());
            case "edit": return Edit(args.Skip(1).ToList());
            case "list": return List(args.Skip(1).ToList());
            case "listcat": return ListCat(args.Skip(1).ToList());
            case "preset": return AddByPreset(args.Skip(1).ToList());
            case "sum": return Sum(args.Skip(1).ToList());
            case "help": return LongHelp;
            default: return $"Unknown argument 'food {args[0]}', type 'help food'";
        }
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count < 6) return "Not Enough Arguments, usage: food add [item name] [amount] [calories] [protein] [carbs] [fat] (category) (notes)";

        string itemName = args[0];
        if (!double.TryParse(args[1], out double amount) ||
            !double.TryParse(args[2], out double calories) ||
            !double.TryParse(args[3], out double protein) ||
            !double.TryParse(args[4], out double carbs) ||
            !double.TryParse(args[5], out double fat))
        {
            return "Did not parse one or more of the given numbers (amount, calories, protein, carbs, fat).";
        }
        
        string? category = null;
        string? notes = null;
        
        if (args.Count > 6)
        {
            // 7th argument is category (if provided)
            category = string.IsNullOrWhiteSpace(args[6]) ? null : args[6];
        }
        if (args.Count > 7)
        {
            // 8th argument is notes (if provided)
            notes = string.IsNullOrWhiteSpace(args[7]) ? null : args[7];
        }

        try {return $"Entry added: {PrintEntry(_service.AddEntry(DateTime.Now, itemName, amount, calories, protein, carbs, fat, category, notes))}";}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Add(): Error adding: {ex.Message}";}
    }

    private string Delete(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: food delete [ID]/Last. 'Type food list' to find ID";
        if (args[0] == "last")
        {
            try {return $"Deleted last entry: {PrintEntry(_service.DeleteLast())}";}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Delete(): Error deleting last entry {ex.Message}";}
        }
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        try {return $"Deleted entry: {PrintEntry(_service.DeleteEntry(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Delete(): Error deleting entry {ex.Message}";}
    }

    private string Edit(IReadOnlyList<string> args)
    {
        if (args.Count < 7) return "Not Enough Arguments, usage: food edit [ID] [item name] [amount] [calories] [protein] [carbs] [fat] (category) (notes)";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        string itemName = args[1];
        if (!double.TryParse(args[2], out double amount) ||
            !double.TryParse(args[3], out double calories) ||
            !double.TryParse(args[4], out double protein) ||
            !double.TryParse(args[5], out double carbs) ||
            !double.TryParse(args[6], out double fat))
        {
            return "Did not parse one or more of the given numbers (amount, calories, protein, carbs, fat).";
        }
        
        string? category = null;
        string? notes = null;
        
        if (args.Count > 7)
        {
            // 7th argument is category (if provided)
            category = string.IsNullOrWhiteSpace(args[7]) ? null : args[7];
        }
        if (args.Count > 8)
        {
            // 8th argument is notes (if provided)
            notes = string.IsNullOrWhiteSpace(args[8]) ? null : args[8];
        }

        try {return $"Entry edited: {PrintEntry(_service.EditEntry(id, DateTime.Now, itemName, amount, calories, protein, carbs, fat, category, notes))}";}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Edit(): Error editing: {ex.Message}";}
    }

    private string List(IReadOnlyList<string> args)
    {
        if (args.Count == 0)
        {
            try {return PrintList(_service.GetAll());}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.List(): Error getting list: {ex.Message}";} 
        }
        if (!DateTime.TryParse(args[0], out DateTime start)) return $"Cannot parse start Date/Time '{args[0]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        if (args.Count == 1) 
        {
            try {return PrintList(_service.FromTimes(start, DateTime.Now));}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.List(): Error getting FromTimeList: (StartDate-Now): {ex.Message}";}
        }
        if (!DateTime.TryParse(args[1], out DateTime end)) return $"Cannot end parse Date/Time '{args[1]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        try {return PrintList(_service.FromTimes(start, end));}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.List(): Error getting FromTimeList: (StartDate-EndDate): {ex.Message}";}
    }

    private string ListCat(IReadOnlyList<string> args)
    {
        string? searchKey;
        if (args.Count == 0) searchKey = null;
        else searchKey = args[0];
        try {return PrintList(_service.GetAllCategory(searchKey));}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.ListCat(): Error getting category list, empty list or none with category probably: {ex.Message}";}
    }

    private string AddByPreset(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: food preset [ID] (multiplier) (notes)";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        if (args.Count == 1)
        {
            try {return PrintEntry(_service.AddByPreset(DateTime.Now, id));}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.AddByPreset(): Error adding by preset [ID], wrong ID probably. Try 'preset list' to see IDs: {ex.Message}";}
        }
        if (!double.TryParse(args[1], out double multiplier)) return $"Invalid multiplier '{args[1]}', must be a number (double)'";
        if (args.Count == 2)
        {
            try {return PrintEntry(_service.AddByPreset(DateTime.Now, id, multiplier));}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.AddByPreset(): Error adding by preset [ID] (multiplier), wrong ID probably. Try 'preset list' to see IDs: {ex.Message}";}
        }
        try {return PrintEntry(_service.AddByPreset(DateTime.Now, id, multiplier, args[2]));}
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.AddByPreset(): Error adding by preset [ID] (multiplier) (notes), wrong ID probably. Try 'preset list' to see IDs: {ex.Message}";}
    }
    
    private string Sum(IReadOnlyList<string> args)
    {
        if (args.Count < 2) return "Not enough arguments, usage: food sum [parameter] {start time] (end time)";        
        if (!DateTime.TryParse(args[1], out DateTime start)) return $"Cannot end parse Date/Time '{args[1]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        if (args.Count == 2)
        {
            try {return _service.PeriodSum(args[0], start, DateTime.Now).ToString();}
            catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Sum(): Error producing sum: {ex.Message}";}
        }
        if (!DateTime.TryParse(args[2], out DateTime end)) return $"Cannot end parse Date/Time '{args[2]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        try { return _service.PeriodSum(args[0], start, end).ToString(); }
        catch (Exception ex) {return $"Puppet.Commands.FoodCommand.Sum(): Error producing sum: {ex.Message}";}
    }

    private string PrintEntry(FoodEntry entry)
    {
        // Simple readable columns
        return $"{entry.Id,3} {entry.Time:yyyy/MM/dd HH:mm:ss} {entry.ItemName,-15} {entry.Amount,5:0.0} {entry.Calories,7:0} {entry.Protein,7:0.0} {entry.Carbs,7:0.0} {entry.Fat,7:0.0} {(entry.Category ?? "N/A"),-10} {entry.Notes ?? ""}";
    }

    private string PrintList(List<FoodEntry> FoodList)
    {
        if (FoodList == null || FoodList.Count == 0)
            return "No food entries found.";

        string listString = $"ID  Time                ItemName        Amount Calories Protein  Carbs    Fat     Category   Notes\n";
        foreach (var entry in FoodList)
        {
            listString += PrintEntry(entry) + "\n";
        }
        return listString;
    }
}