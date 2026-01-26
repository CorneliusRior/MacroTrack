namespace MacroTrack.Core.Services;

using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class WeightLogService
{
    private readonly WeightLogRepo _repo;

    public event EventHandler<string> RequestPrint;
    public event EventHandler<string> RequestPrintInline;

    public WeightLogService(WeightLogRepo repo)
    {
        _repo = repo;
        _repo.RequestPrint += (sender, text) => RepoPrint(sender!, text);
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

    private void Print(string text, [CallerMemberName] string caller = "")
    {
        RequestPrint?.Invoke(this, $"{caller}(): {text}");
    }

    private void RepoPrint(object sender, string text)
    {
        RequestPrint?.Invoke(sender, text);
    }

    private void PrintInline(string text)
    {
        RequestPrintInline?.Invoke(this, text);
    }
}