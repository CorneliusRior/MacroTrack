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
        if (added == null) throw new Exception("Core.Services.PresetService.AddEntry(): Cannot find entry");
        return added;
    }

    // load
    public Preset? GetEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.PresetService.GetEntry(): Cannot find entry");
        return entry;
    }

    // Load all
    public List<Preset> GetAll()
    {
        var entryList = _repo.GetAll();
        return entryList;
    }

    // Get all in category
    public List<Preset> GetAllCategory(string? category)
    {
        var entryList = _repo.GetAllCategory(category);
        if (entryList == null) throw new Exception("Core.services.PresetService.GetAllCategory(): No entries returned.");
        return entryList;
    }

    // Get all categories
    public List <string> GetCategoryList()
    {
        return _repo.GetCategoryList();
    }

    // Load all names
    public List<string>? GetAllNames()
    {
        var names = _repo.GetAllNames();
        if (names == null) throw new Exception("Core.Services.PresetService.GetAllNames(): Cannot find names");
        return names;
    }

    // Update (replace tbh but update)
    public Preset? EditEntry(int id, string presetName, double calories, double protein, double carbs, double fat, double? weight, string? unit, string? category, string? notes)
    {
        var entry = new Preset(presetName, calories, protein, carbs, fat, weight, unit, category, notes);
        _repo.EditEntry(id, entry);
        var edited = _repo.GetEntry(id);
        if (edited == null) throw new Exception("Core.Services.PresetService.EditEntry(): Cannot find entry");
        return edited;
    }

    // delete
    public Preset? DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.PresetService.DeleteEntry(): Cannot find entry");
        _repo.DeleteEntry(id);
        return entry;
    }
}