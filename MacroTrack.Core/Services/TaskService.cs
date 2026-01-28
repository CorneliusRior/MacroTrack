namespace MacroTrack.Core.Services;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Repositories;

using System.Runtime.CompilerServices;

/// <summary>
/// Service for interacting with Daily Task data and repo, TaskCompletion, TaskLog and TaskRegistry. Add, retrieve, delete, set completion, activate or deactive &c.
/// </summary>
/// <remarks>
/// Not updated for loggin.
/// </remarks>
public class TaskService : ServiceBase
{
    private readonly TaskRepo _repo;

    public TaskService(TaskRepo repo, CoreContext ctx) : base(ctx)
    {
        _repo = repo;
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
        if (addedTask == null) throw new Exception("Core.Services.TaskService.AddTask(): Error adding entry, returned as null. Not saved or not read.");
        return addedTask;
    }

    // Load 1
    public DailyTask GetTask(int id, DateTime? date = null)
    {
        var entry = _repo.GetTask(id, date ?? DateTime.Now);
        if (entry == null) throw new Exception($"Core.Services.TaskService.GetTask(): Error getting entry, returned as null. Entry probably doesn't exist, or wrong ID: [{id}].");
        return entry;
    }

    // Load all
    public List<DailyTask> GetAll(DateTime? date = null, bool filterActive = false, bool filterInactive = false)
    {
        List<DailyTask> taskList = _repo.GetAll(date ?? DateTime.Now);
        if (filterActive == true) { taskList.RemoveAll(t => t.IsActive); }
        if (filterInactive == true) { taskList.RemoveAll(t => !t.IsActive); }
        return taskList;
    }

    // Load all with streaks
    public List<DailyTask> GetAllStreaks(DateTime? date = null, bool filterActive = false, bool filterInactive = false)
    {
        List<DailyTask> taskList = _repo.GetAllStreaks(date ?? DateTime.Now);
        if (filterActive == true) { taskList.RemoveAll(t => t.IsActive); }
        if (filterInactive == true) { taskList.RemoveAll(t => !t.IsActive); }
        return taskList;
    }

    // Load Registry list
    public List<DailyTask> RegistryList(bool filterInactive = false, bool filterActive = false)
    {
        var taskList = _repo.GetRegistry();
        if (filterActive == true) {taskList.RemoveAll(t => t.IsActive);}
        if (filterInactive == true) {taskList.RemoveAll(t => !t.IsActive);}
        return taskList;
    }

    // Set complete
    public DailyTask SetComplete(int id, DateTime? date = null)
    {
        _repo.SetComplete(id, date ?? DateTime.Now);
        var entry = GetTask(id, date);
        return entry;
    }

    // Set incomplete
    public DailyTask SetIncomplete(int id, DateTime? date = null)
    {
        _repo.SetIncomplete(id, date ?? DateTime.Now);
        var entry = GetTask(id, date);
        return entry;
    }

    // disable
    public DailyTask Activate(int id)
    {
        _repo.ActivateEntry(id);
        return GetTask(id, DateTime.Now);
    }

    // reenable
    public DailyTask Deactivate(int id)
    {
        _repo.DeactivateEntry(id);
        return GetTask(id, DateTime.Now);
    }
}