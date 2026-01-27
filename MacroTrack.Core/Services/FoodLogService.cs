namespace MacroTrack.Core.Services;

using MacroTrack.Core.AppModels;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class FoodLogService : ServiceBase
{
    private readonly FoodLogRepo _repo;
    private readonly PresetRepo _presetRepo;

    public FoodLogService(FoodLogRepo repo, PresetRepo presetRepo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
        _presetRepo = presetRepo;;
    }

    // New 
    public FoodEntry? AddEntry(DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        var entry = new FoodEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
        _repo.AddEntry(entry);
        var added = _repo.GetEntry(_repo.ReturnLastId());
        if (added == null) throw new Exception("Core.Services.FoodLogService.AddEntry(): Cannot find entry.");
        return added;
    }

    // Add by preset
    public FoodEntry AddByPreset(DateTime time, int id, double amount = 1, string? notes = null)
    {
        var preset = _presetRepo.GetEntry(id);
        if (preset == null) throw new Exception("Core.Services.FoodLogService.AddByPreset(): Cannot find preset.");
        var entry = new FoodEntry(time, preset.PresetName, amount, preset.Calories * amount, preset.Protein * amount, preset.Carbs * amount, preset.Fat * amount, preset.Category, notes);
        _repo.AddEntry(entry);
        var added = _repo.GetEntry(_repo.ReturnLastId());
        if (added == null) throw new Exception("Core.Services.FoodLogService.AddByPreset(): Entry returned as null.");
        return added;
    }
    
    // Load 1
    public FoodEntry GetEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.FoodLogService.GetEntry(): null entry, wrong ID probably.");
        return _repo.GetEntry(id);
    }

    // Load all
    public List<FoodEntry>? GetAll()
    {
        var entryList = _repo.GetAll();
        if (entryList == null) throw new Exception("Core.Services.FoodLogService.GetAll(): null entry list, no entries probably");
        return entryList;
    }

    // Load all in category
    public List<FoodEntry>? GetAllCategory(string? category)
    {
        var entryList = _repo.GetAllCategory(category);
        if (entryList == null) throw new Exception("Core.Services.FoodLogService.GetAllCategory(): null entry list, no entries probably"); // I do wonder if we should be throwing exceptions every time, like a GUI will just need to display "empty" right? A problem we can deal with later I guess, as tedious as it will be.
        return entryList;
    }

    // Load selection
    public List<FoodEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        var entryList = _repo.FromTimes(startTime, endTime);
        if (entryList == null) throw new Exception("Core.Services.FoodLogService.FromTimes(): null entry list, no entries probably");
        return entryList;
    }

    
    // Period sum
    public double PeriodSum(string parameter, DateTime startTime, DateTime endTime)
    {
        // make sure parameter is lower case first letter capitalisedL
        parameter = parameter.Substring(0, 1).ToUpper() + parameter.Substring(1).ToLower();
        // varify parameter is valid:
        if (parameter != "Amount" && parameter != "Calories" && parameter != "Protein" && parameter != "Carbs" && parameter != "Fat")
        {
            throw new Exception("Core.Services.FoodLogService.PeriodSum(): invalid parameter, probably a typo, must be \"Amount\", \"Calories\", \"Protein\", \"Carbs\", or \"Fat\"");
        }
        var sum = _repo.PeriodSum(parameter, startTime, endTime);
        if (double.IsNaN(sum)) throw new Exception("Core.Services.FoodLogService.PeriodSum(): null sum, wrong ID probably.");
        return sum;
    }

    // 
    public MacroTotals GetMacroTotals(DateTime startTime, DateTime endTime)
    {
        var totals = new MacroTotals
        {
            Calories = PeriodSum("Calories", startTime, endTime),
            Protein = PeriodSum("Protein", startTime, endTime),
            Carbs = PeriodSum("Carbs", startTime, endTime),
            Fat = PeriodSum("Fat", startTime, endTime)
        };
        return totals;
    }

    // Edit
    public FoodEntry? EditEntry(int id, DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception($"Core.Services.FoodLogServices.EditEntry(): null entry for entry [{id}]: probably doesn't exist.");
        category = category ?? entry.Category;
        notes = notes ?? entry.Notes;
        notes += $"{Environment.NewLine}{Environment.NewLine}Edited at {DateTime.Now.ToString("u")}";
        var newEntry = new FoodEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
        _repo.EditEntry(id, newEntry);
        return _repo.GetEntry(id);
    }

    // delete
    public FoodEntry? DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.FoodLogService.DeleteEntry(): null entry, wrong ID probably.");
        _repo.DeleteEntry(id);
        return entry;
    }

    // delete last
    public FoodEntry? DeleteLast()
    {
        return DeleteEntry(_repo.ReturnLastId());
    }
}