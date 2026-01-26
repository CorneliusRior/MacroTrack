using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class PresetCommand : ICommand
{
    private readonly PresetService _service;

    public string Name => "preset";
    public IReadOnlyList<string> Aliases => new[] { "p", "presets" };
    public string Description => "Commands for interfacing with Presets.";
    public string Usage => "preset [add/edit/delete/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with Presets. Subcommands are:
     - preset add [name] [calories] [protein] [carbs] [fat] (weight) (unit) (category) (notes) 
                                             - Creates a new preset. First 5 arguments required, rest optional. Space between double quotes to leave a field blank.
     - preset category                       - Returns list of categories present.
     - preset delete [id]                    - Deletes the selected preset. No (Y/N) confirmation.
     - preset edit [id] [name] [calories] [protein] [carbs] [fat] (weight) (unit) (category) (notes)  
                                             - allows you to edit a preset. First 6 arguments required (id + 5), rest optional. Space between double quotes to leave a field blank.
     - preset list (category)                - Lists all presets, or lists all presets in string 'category' if provided.";

    public PresetCommand(PresetService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return ("Not enough arguments, usage: " + Usage);
        switch (args[0])
        {
            case "add": return Add(args.Skip(1).ToList());
            case "edit": return Edit(args.Skip(1).ToList());
            case "delete": return Delete(args.Skip(1).ToList());
            case "category": return Category(args.Skip(1).ToList());
            case "list": return List(args.Skip(1).ToList());
            case "help": return LongHelp;
            default: return ("Not enough arguments, usage: " + Usage);
        }
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count < 5) return ("Not enough arguments, usage: preset add [name] [calories] [protein] [carbs] [fat] (weight) (unit) (category) (notes)");

        string presetName = args[0];
        if (!double.TryParse(args[1], out double calories) ||
            !double.TryParse(args[2], out double protein) ||
            !double.TryParse(args[3], out double carbs) ||
            !double.TryParse(args[4], out double fat)) 
        {
            return "Did not parse one or more of the given numbers (calories, protein, carbs, fat)";
        }
        
        double? weight = null;
        string? unit = null;
        string? category = null;
        string? notes = null;
        
        if (args.Count > 5 && double.TryParse(args[5], out double parsedWeight)) weight = parsedWeight;
        if (args.Count > 6) unit = string.IsNullOrWhiteSpace(args[6]) ? null : args[6];
        if (args.Count > 7) category = string.IsNullOrWhiteSpace(args[7]) ? null : args[7];
        if (args.Count > 8) notes = string.IsNullOrWhiteSpace(args[8]) ? null : args[8];
        
        try {return $"Preset added: {PrintEntry(_service.AddEntry(presetName, calories, protein, carbs, fat, weight, unit, category, notes))}";}
        catch (Exception ex) {return $"Puppet.Commands.PresetCommand.Add(): Error adding preset: {ex.Message}";}
    }

    private string Category(IReadOnlyList<string> args)
    {
        try {return PrintStringList(_service.GetCategoryList());}
        catch (Exception ex) {return $"Puppet.Commands.PresetCommand.PrintList(): Error getting category list, no categories probably: {ex.Message}";}
    }

    private string Delete(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: preset delete [ID]";
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer";
        try {return $"Deleted preset: {PrintEntry(_service.DeleteEntry(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.PresetCommand.Delete(): Error deleting preset: {ex.Message}";}
    }

    private string Edit(IReadOnlyList<string> args)
    {
        if (args.Count < 6) return ("Not enough arguments, usage: preset edit [id] [name] [calories] [protein] [carbs] [fat] (weight) (unit) (category) (notes)");
        if (!int.TryParse(args[0], out int id)) return $"Cannot parse ID '{args[0]}', must be an integer.";
        
        Preset? oldEntry;
        try {oldEntry = _service.GetEntry(id);}
        catch {return $"Invalid ID '{args[0]}', entry not found, type 'preset list' to see IDs";}
        
        string presetName = args[1];
        if (!double.TryParse(args[2], out double calories) ||
            !double.TryParse(args[3], out double protein) ||
            !double.TryParse(args[4], out double carbs) ||
            !double.TryParse(args[5], out double fat)) 
        {
            return "Did not parse one or more of the given numbers (calories, protein, carbs, fat)";
        }
        
        double? weight = null;
        string? unit = null;
        string? category = null;
        string? notes = null;
        
        if (args.Count > 6 && double.TryParse(args[6], out double parsedWeight)) weight = parsedWeight;
        if (args.Count > 7) unit = string.IsNullOrWhiteSpace(args[7]) ? null : args[7];
        if (args.Count > 8) category = string.IsNullOrWhiteSpace(args[8]) ? null : args[8];
        if (args.Count > 9) notes = string.IsNullOrWhiteSpace(args[9]) ? null : args[9];
        
        try 
        {
            var editedEntry = _service.EditEntry(id, presetName, calories, protein, carbs, fat, weight, unit, category, notes);
            return $"Preset:\n{PrintEntry(oldEntry)}\n\nEdited to:\n{PrintEntry(editedEntry)}";
        }
        catch (Exception ex) {return $"Puppet.Commands.PresetCommand.Edit(): Error editing preset: {ex.Message}";}
    }

    private string List(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return PrintList(_service.GetAll());
        try {return PrintList(_service.GetAllCategory(args[0]));}
        catch (Exception ex) {return $"Puppet.Commands.PresetCommand.List(): Error getting category filter list, category probably not present: {ex.Message}";}
    }

    private string PrintEntry(Preset entry)
    {
        return $"{$"[{entry.Id}]",5} {entry.PresetName,-18} {entry.Calories,8:0} {entry.Protein,8:0.0} {entry.Carbs,8:0.0} {entry.Fat,8:0.0} {(entry.Weight.HasValue ? entry.Weight.Value.ToString("0.0") : "N/A"),8} {(entry.Unit ?? "N/A"),-5} {(entry.Category ?? "N/A"),-10} {entry.Notes ?? ""}";
    }

    private string PrintList(List<Preset> presetList)
    {
        if (presetList == null || presetList.Count == 0)
        {
            return "No presets found.";
        }

        string listString = "ID  Name                Calories  Protein  Carbs    Fat      Weight   Unit  Category    Notes\n";
        foreach (var entry in presetList)
        {
            listString += PrintEntry(entry) + "\n";
        }
        return listString;
    }

    private string PrintStringList(List<string> stringList)
    {
        if (stringList == null || stringList.Count == 0) return "No strings found";
        string listString = "";
        foreach (string s in stringList) listString += s + "\n";
        return listString;
    }
}