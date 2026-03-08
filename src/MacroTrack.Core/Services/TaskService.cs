namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// Service for interacting with Daily Task data and repo, TaskCompletion, TaskLog and TaskRegistry. Add, retrieve, delete, set completion, activate or deactive &c.
/// </summary>
/// <remarks>
/// Updated for logging.
/// </remarks>
public class TaskService : ServiceBase
{
    private readonly TaskRepo _repo;

    public TaskService(TaskRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
    }

    // Cheat Days:
    public void SetCheatDay(DateTime date, bool isCheatDay)
    {
        _repo.SetCheatDay(date, isCheatDay);
    }

    public bool GetIsCheatDay(DateTime date)
    {
        return _repo.GetIsCheatDay(date);
    }

    public List<DateTime> GetCheatDayRange(DateTime startTime, DateTime endTime)
    {
        return _repo.GetCheatDayRange(startTime, endTime);
    }

    public List<(DateTime date, double value)> GetCheatDayTupleRange(DateTime startTime, DateTime endTime)
    {
        List<(DateTime date, double value)> returnList = new();
        List<DateTime> range = _repo.GetCheatDayRange(startTime, endTime);
        foreach (var t in range) returnList.Add((t, 1));
        return returnList;
    }

    // Add
    public DailyTask AddTask(string name, string? description = null)
    {
        DailyTask newTask;
        if (string.IsNullOrEmpty(description))
            newTask = new DailyTask(name);
        else
            newTask = new DailyTask(name, description);

        _repo.AddTask(newTask);
        var addedTask = _repo.GetTask(_repo.ReturnLastId(), DateTime.Now);
        if (addedTask == null)
        {
            var ex = new Exception("Core.Services.TaskService.AddTask(): Error adding entry, returned as null. Not saved or not read.");
            Log("Error adding task", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Added Task #{addedTask.Id}");
        return addedTask;
    }

    // Load 1
    public DailyTask GetTask(int id, DateTime? date = null)
    {
        var entry = _repo.GetTask(id, date ?? DateTime.Now);
        if (entry == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error retrieving Task #{id}, probably doesn't exist");
            throw ex;
        }
        Log($"Requested Task #{id}");
        return entry;
    }

    // Load all
    public List<DailyTask> GetAll(DateTime? date = null, bool filterActive = false, bool filterInactive = false)
    {
        List<DailyTask> taskList = _repo.GetAll(date ?? DateTime.Now);
        if (filterActive == true) { taskList.RemoveAll(t => t.IsActive); }
        if (filterInactive == true) { taskList.RemoveAll(t => !t.IsActive); }
        Log($"Requested TaskList for '{(date is null ? "Null (now)" : date)}', filterActive = '{filterActive}', filterInactive = '{filterInactive}'");
        return taskList;
    }

    // Load all with streaks
    public List<DailyTask> GetAllStreaks(DateTime? date = null, bool filterActive = false, bool filterInactive = false)
    {
        List<DailyTask> taskList = _repo.GetAllStreaks(date ?? DateTime.Now);
        if (filterActive == true) { taskList.RemoveAll(t => t.IsActive); }
        if (filterInactive == true) { taskList.RemoveAll(t => !t.IsActive); }
        Log($"Requested TaskList for DateTime '{(date is null ? "Null (now)" : date)}', filterActive = '{filterActive}', filterInactive = '{filterInactive}'");
        return taskList;
    }

    // Load Registry list
    public List<DailyTask> RegistryList(bool filterInactive = false, bool filterActive = false)
    {
        var taskList = _repo.GetRegistry();
        if (filterActive == true) {taskList.RemoveAll(t => t.IsActive);}
        if (filterInactive == true) {taskList.RemoveAll(t => !t.IsActive);}
        Log($"Requested TaskRegistry, filterActive = '{filterActive}', filterInactive = '{filterInactive}'");
        return taskList;
    }

    // Set complete
    public DailyTask SetComplete(int id, DateTime? date = null)
    {
        _repo.SetComplete(id, date ?? DateTime.Now);
        var entry = _repo.GetTask(id, date ?? DateTime.Now);
        if (entry == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error setting Task #{id} complete, probably doesn't exist, no edits made.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Set Task #{id} Complete for DateTime '{(date is null ? "Null (now)" : date)}'");
        return entry;
    }

    // Set incomplete
    public DailyTask SetIncomplete(int id, DateTime? date = null)
    {
        _repo.SetIncomplete(id, date ?? DateTime.Now);
        var entry = _repo.GetTask(id, date ?? DateTime.Now);
        if (entry == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error setting Task #{id} incomplete, probably doesn't exist, no edits made.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Set Task #{id} Incomplete for DateTime '{(date is null ? "Null (now)" : date)}'");
        return entry;
    }

    // disable    
    public DailyTask Deactivate(int id)
    {
        _repo.DeactivateEntry(id);
        var entry = _repo.GetTask(id, DateTime.Now);
        if (entry == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error deactivating Task #{id}, probably doesn't exist, no edits made.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Deactivated Task #{id}");
        return entry;
    }

    // reenable
    public DailyTask Activate(int id)
    {
        _repo.ActivateEntry(id);
        var entry = _repo.GetTask(id, DateTime.Now);
        if (entry == null)
        {
            var ex = new Exception($"Returned as null.");
            Log($"Error activating Task #{id}, probably doesn't exist, no edits made.", LogLevel.Warning, ex);
            throw ex;
        }
        Log($"Activated Task #{id}");
        return entry;
    }
}