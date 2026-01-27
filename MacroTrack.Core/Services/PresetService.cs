namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class PresetService : ServiceBase
{
    private readonly PresetRepo _repo;

    public PresetService(PresetRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    // new
    public Preset AddEntry(string presetName, double calories, double protein, double carbs, double fat, double? weight, string? unit, string? category, string? notes)
    {
        var entry = new Preset(presetName, calories, protein, carbs, fat, weight, unit, category, notes);
        _repo.AddEntry(entry);
        var added = _repo.GetEntry(_repo.ReturnLastId());
        if (added == null)
        {
            var ex = new Exception("Entry returned as null.");
            Log("Error retrieving entry after adding.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Added preset #{added.Id}");
        return added;
    }

    // load
    public Preset? GetEntry(int id)
    {
        Log($"Request preset #{id}");
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
    public List<Preset> GetAll()
    {
        Log();
        return _repo.GetAll();         
    }

    // Get all in category
    public List<Preset> GetAllCategory(string? category)
    {
        Log();
        return _repo.GetAllCategory(category);
    }

    // Get all categories
    public List <string> GetCategoryList()
    {
        Log();
        return _repo.GetCategoryList();
    }

    // Load all names
    public List<string>? GetAllNames()
    {
        Log();
        return _repo.GetAllNames();
    }

    // Update (replace tbh but update)
    public Preset EditEntry(int id, string presetName, double calories, double protein, double carbs, double fat, double? weight, string? unit, string? category, string? notes)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception($"Null entry for entry [{id}].");
            Log($"Error editing entry #{id}, probably doesn't exist.", LogLevel.Warning, ex);
            throw ex;
        }
        var newEntry = new Preset(presetName, calories, protein, carbs, fat, weight, unit, category, notes);
        _repo.EditEntry(id, newEntry);
        Log($"Edited preset #{newEntry.Id} '{newEntry.PresetName}'");
        return _repo.GetEntry(id)!;
    }

    // delete
    public Preset DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Entry returned as null.");
            Log("Error deleting entry, no deletion has been made, wrong ID probably.", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteEntry(id);
        Log($"Deleted entry #{entry.Id}");
        return entry;
    }
}