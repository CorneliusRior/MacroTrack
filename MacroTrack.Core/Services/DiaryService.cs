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
        Log("New entry requested.");
        _repo.AddEntry(new DiaryEntry(DateTime.Now, body));
        var entry = _repo.GetEntry(_repo.ReturnLastId());
        if (entry == null)
        {
            Exception ex = new Exception("Entry returned as null after adding, connection issue?");
            Log("Errrr addubg entry.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Added entry {entry.Id}.", LogLevel.Info);
        return entry;
    }

    public DiaryEntry EditEntry(int id, string body, string editNotes)
    {
        Log($"Edit to entry #{id} requested");
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
        return entry!;
    }

    public DiaryEntry GetEntry(int id)
    {
        Log($"Diary entry #{id} requested");
        var entry = _repo.GetEntry(id);
        if (entry == null) throw new Exception("Entry returned as null, not found.");
        return entry;
    }

    public List<DiaryEntry> GetAll()
    {
        Log("All diary entries requested");
        return _repo.GetAll();
    }

    public List<DiaryEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        Log($"Diary entries requested form times: {startTime} to {endTime}");
        return _repo.FromTimes(startTime, endTime);
    }

    public DiaryEntry DeleteEntry(int id)
    {
        Log($"Entry deletion on entry #{id} requested");
        var entry = _repo.GetEntry(id);
        if (entry == null)
        {
            var ex = new Exception("Entry returned as null, not found.");
            Log($"Error deleting entry #{id}, probably doesn't exist.", LogLevel.Warning, ex);
            throw ex;
        }
        _repo.DeleteEntry(id);
        Log($"Deleted entry #{id}", LogLevel.Info);
        return entry;
    }

    public DiaryEntry DeleteLast()
    {
        Log($"DeleteLast Requested");
        return DeleteEntry(_repo.ReturnLastId());
    }

}