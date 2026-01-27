namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class WeightLogService : ServiceBase
{
    private readonly WeightLogRepo _repo;

    public WeightLogService(WeightLogRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    // New
    public WeightEntry AddEntry(DateTime time, double weight)
    {
        _repo.AddEntry(new WeightEntry(time, weight));
        var addedEntry = _repo.GetEntry(_repo.ReturnLastId());
        if (addedEntry == null) throw new Exception("Core.Services.WeightLogService.AddEntry(): Error adding entry. returned as null. Not saved or not read.");
        return addedEntry;
    }

    // Load
    public WeightEntry? GetEntry(int id)
    {
        return _repo.GetEntry(id);
    }

    // Load all
    public List<WeightEntry> GetAll()
    {
        return _repo.GetAll();
    }

    // Load selection
    public List<WeightEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        return _repo.FromTimes(startTime, endTime);        
    }

    // Delete
    public WeightEntry DeleteEntry(int id)
    {
        var DeletedEntry = _repo.GetEntry(id);
        if (DeletedEntry == null) throw new Exception("Core.Services.WeightLogService.DeleteEntry(): Entry not found");
        _repo.DeleteEntry(id);
        return DeletedEntry;
    }

    // Delete last
    public WeightEntry DeleteLast()
    {
        return DeleteEntry(_repo.ReturnLastId());
    }
}