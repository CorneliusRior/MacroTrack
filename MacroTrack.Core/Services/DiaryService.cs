namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

public class DiaryService : ServiceBase
{
    private readonly DiaryRepo _repo;

    public DiaryService(DiaryRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    public DiaryEntry AddEntry(string body)
    {
        _repo.AddEntry(new DiaryEntry(DateTime.Now, body));
        var entry = _repo.GetEntry(_repo.ReturnLastId());
        if (entry == null)
        {
            Exception ex = new Exception("Entry returned as null after adding, connection issue?");
            Log("Errir addubg entry.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Added entry {entry.Id}.", LogLevel.Info);
        return entry;
    }

    public DiaryEntry EditEntry(int id, string body, string editNotes)
    {
        // some clarification: The edit feature allows you to edit the entire body, places a "edited" timestamp at the bottom, and allows you to put "edit notes" at the end.
        _repo.EditEntry(id, body + $"{Environment.NewLine}{Environment.NewLine}Edited on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +  Environment.NewLine
        + editNotes);
        var entry = _repo.GetEntry(id);

        if (entry == null)
        {
            Exception ex = new Exception("Entry returned as null after editing, connection issue?.");
            Log($"Error editing entry #{id}.", LogLevel.Warning, ex);
            throw ex;
        }

        Log($"Edited entry {entry.Id}", LogLevel.Info);
        return entry;
    }

    public DiaryEntry? GetEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.DiaryService.GetEntry(): Entry not found");
        return entry;
    }

    public List<DiaryEntry> GetAll()
    {
        return _repo.GetAll();
    }

    public List<DiaryEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        Log("Get all called()");
        return _repo.FromTimes(startTime, endTime);
    }

    public DiaryEntry? DeleteEntry(int id)
    {
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Core.Services.DiaryService.DeleteEntry(): Entry not found.");
        _repo.DeleteEntry(id);
        return entry;
    }

    public DiaryEntry? DeleteLast()
    {
        return DeleteEntry(_repo.ReturnLastId());
    }

}