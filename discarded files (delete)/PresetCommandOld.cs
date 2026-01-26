using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class PresetCommandOld : ICommand
{
    private readonly PresetService _service;

    public string Name => "preset";
    public IReadOnlyList<string> Aliases => new[] { "p", "presets" };
    public string Description => "Commands for interfacing with Presets.";
    public string Usage => "preset [new/delete/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with Presets. Subcommands are:
     - preset new [name] [calories] [protein] [carbs] [fat] [weight] [unit] [category] [notes] - Creates a new preset. All arguments required.
     - preset delete [id]                    - Deletes the selected preset. No (Y/N) confirmation.
     - preset list                           - Lists all presets.";

    public PresetCommandOld(PresetService service)
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
            case "new":
                if (args.Count < 10)
                {
                    return ("Not enough arguments, usage: preset new [name] [calories] [protein] [carbs] [fat] [weight] [unit] [category] [notes]");
                }

                string presetName = args[1];
                if (!double.TryParse(args[2], out double calories) ||
                    !double.TryParse(args[3], out double protein) ||
                    !double.TryParse(args[4], out double carbs) ||
                    !double.TryParse(args[5], out double fat) ||
                    !double.TryParse(args[6], out double weight))
                {
                    return "Did not parse one or more of the given numbers (calories, protein, carbs, fat, weight).";
                }

                string unit = args[7];
                string category = args[8];
                string notes = args.Count > 9 ? args[9] : "";

                try
                {
                    var entry = _service.AddEntry(presetName, calories, protein, carbs, fat, weight, unit, category, notes);
                    return $"Preset added: {PrintEntry(entry)}";
                }
                catch (Exception ex)
                {
                    return "Error adding preset: " + ex.Message;
                }

            case "delete":
                if (args.Count < 2)
                {
                    return "Not enough arguments, usage: preset delete [ID]. Type 'preset list' to find ID";
                }

                if (!int.TryParse(args[1], out int id))
                {
                    return $"Invalid ID '{args[1]}', must be an integer.";
                }

                try
                {
                    var deletedEntry = _service.DeleteEntry(id);
                    return $"Deleted preset: {PrintEntry(deletedEntry)}";
                }
                catch (Exception ex)
                {
                    return "Error deleting preset: " + ex.Message;
                }

            case "list":
                try
                {
                    return PrintList(_service.GetAll());
                }
                catch (Exception ex)
                {
                    return "Error getting list: " + ex.Message;
                }

            default:
                return $"Unknown argument 'preset {args[0]}', type 'help preset'.";
        }
    }

    private string PrintEntry(Preset entry)
    {
        return $"{$"[{entry.Id}]",5} {entry.PresetName,-18} {entry.Calories,8:0} {entry.Protein,8:0.0} {entry.Carbs,8:0.0} {entry.Fat,8:0.0} {entry.Weight,8:0.0} {entry.Unit,-5} {entry.Category,-10} {entry.Notes}";
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
            listString += $"{entry.Id,3} {entry.PresetName,-18} {entry.Calories,8:0} {entry.Protein,8:0.0} {entry.Carbs,8:0.0} {entry.Fat,8:0.0} {entry.Weight,8:0.0} {entry.Unit,-5} {entry.Category,-10} {entry.Notes}\n";
        }
        return listString;
    }
}
