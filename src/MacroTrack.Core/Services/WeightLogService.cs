namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

/// <summary>
/// Service for interacting with WeightLog data and repo, add, retrieve, and delete Weight Entries
/// </summary>
/// <remarks>
/// The first of the various services made if memory serves.
/// Updated for logging.
/// </remarks>
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
        if (addedEntry == null)
        {
            var ex = new Exception("Returned as null.");
            Log("Error adding entry.", LogLevel.Warning, ex);
            throw ex;
        }
        Log();
        return addedEntry;
    }

    // Load
    public WeightEntry? GetEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Returned as null.");
            Log("Error getting entry, wrong ID probably", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Requested entry #{id}");
        return entry;
    }

    // Load all
    public List<WeightEntry> GetAll()
    {
        Log();
        return _repo.GetAll();
    }

    // Load selection
    public List<WeightEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        Log($"Times '{startTime}' to '{endTime}'");
        return _repo.FromTimes(startTime, endTime);        
    }

    // Delete
    public WeightEntry DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Returned as null.");
            Log("Error deleting entry, wrong ID probably", LogLevel.Warning, ex);
            throw ex;
        }        
        _repo.DeleteEntry(id);
        Log($"Deleted entry #{id}");
        return entry;
    }

    // Delete last
    public WeightEntry DeleteLast()
    {
        var entry = _repo.GetEntry(_repo.ReturnLastId());
        if (entry == null)
        {
            var ex = new Exception("Returned as null.");
            Log("Error deleting entry, wrong ID probably", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteEntry(entry.Id);
        Log($"Deleted entry #{entry.Id}");
        return entry;
    }
}