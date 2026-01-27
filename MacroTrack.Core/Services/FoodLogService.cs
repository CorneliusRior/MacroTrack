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
    public FoodEntry AddEntry(DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        var entry = new FoodEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
        _repo.AddEntry(entry);
        var added = _repo.GetEntry(_repo.ReturnLastId());
        if (added == null)
        {
            var ex = new Exception("Cannot find entry.");
            Log("Error adding entry, wrong ID probably", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Added entry #{entry.Id}");
        return added;
    }

    // Add by preset
    public FoodEntry AddByPreset(DateTime time, int id, double amount = 1, string? notes = null)
    {
        var preset = _presetRepo.GetEntry(id);
        if (preset == null)
        {
            var ex = new Exception("Preset retirmed as null.");
            Log("Error getting preset, wrong ID probably.", LogLevel.Warning, ex);
            throw ex;
        }

        var entry = new FoodEntry(time, preset.PresetName, amount, preset.Calories * amount, preset.Protein * amount, preset.Carbs * amount, preset.Fat * amount, preset.Category, notes);
        _repo.AddEntry(entry);

        var added = _repo.GetEntry(_repo.ReturnLastId());
        if (added == null)
        { 
            var ex = new Exception("Entry returned as null.");
            Log("Error retrieving entry after adding.", LogLevel.Warning, ex);
            throw ex;
        }
        return added;
    }
    
    // Load 1
    public FoodEntry GetEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Entry returned as null.");
            Log("Error getting entry, wrong ID probably.", LogLevel.Warning, ex);
            throw ex;
        }
        else return entry;
    }

    // Load all
    public List<FoodEntry> GetAll()
    {
        Log();
        return _repo.GetAll();
    }

    // Load all in category
    public List<FoodEntry> GetAllCategory(string? category)
    {
        Log($"Category '{(category is null ? category : "null")}' requested.");
        return _repo.GetAllCategory(category);
    }

    // Load selection
    public List<FoodEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        Log($"From times: {startTime} to {endTime}");
        return _repo.FromTimes(startTime, endTime);
    }

    
    // Period sum
    public double PeriodSum(string parameter, DateTime startTime, DateTime endTime)
    {
        Log($"'{parameter}' from: {startTime} to {endTime}");
        // make sure parameter is lower case first letter capitalised
        parameter = parameter.Substring(0, 1).ToUpper() + parameter.Substring(1).ToLower();
        // varify parameter is valid:
        if (parameter != "Amount" && parameter != "Calories" && parameter != "Protein" && parameter != "Carbs" && parameter != "Fat")
        {
            var ex = new Exception($"Invalid parameter '{parameter}', probably a typo, must be \"Amount\", \"Calories\", \"Protein\", \"Carbs\", or \"Fat\"");
            Log("Invalid parameter.", LogLevel.Warning, ex);
            throw ex;
        }
        var sum = _repo.PeriodSum(parameter, startTime, endTime);
        return sum;
    }

    // 
    public MacroTotals GetMacroTotals(DateTime startTime, DateTime endTime)
    {
        Log();
        var totals = new MacroTotals(        
            Calories: PeriodSum("Calories", startTime, endTime),
            Protein: PeriodSum("Protein", startTime, endTime),
            Carbs: PeriodSum("Carbs", startTime, endTime),
            Fat: PeriodSum("Fat", startTime, endTime)
        );
        return totals;
    }

    // Edit
    public FoodEntry EditEntry(int id, DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception($"Null entry for entry [{id}].");
            Log($"Error editing entry #{id}, probably doesn't exist.", LogLevel.Warning, ex);
            throw ex;
        }
        category = category ?? entry.Category;
        notes = notes ?? entry.Notes;
        notes += $"{Environment.NewLine}Edited at {DateTime.Now.ToString("g")}";
        var newEntry = new FoodEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
        _repo.EditEntry(id, newEntry);
        return _repo.GetEntry(id)!;
    }

    // delete
    public FoodEntry DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Entry returned as null.");
            Log("Error deleting entry, no deletion has been made, wrong ID probably.", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteEntry(id);
        return entry;
    }

    // delete last
    public FoodEntry DeleteLast()
    {
        var entry = _repo.GetEntry(_repo.ReturnLastId());
        if (entry == null)
        {
            var ex = new Exception("Entry returned as null.");
            Log("Error deleting last entry, no deletion has been made, list is probably empty", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteEntry(_repo.ReturnLastId());
        return entry;
    }
}