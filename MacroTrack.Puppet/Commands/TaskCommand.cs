using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class TaskCommand : ICommand
{
    private readonly TaskService _service;

    public string Name => "task";
    public IReadOnlyList<string> Aliases => new[] { "t", "tasks", "daily", "dailytasks" };
    public string Description => "Commands for interfacing with Tasks.";
    public string Usage => "task [add/check/list/registry/uncheck] (other arguments)";
    public string LongHelp => @"Command set for interfacing with Daily Tasks. Subcommands are:
     - task add [name] (description)     - adds new task
     - task activate [ID]                - activates a given task (tasks are not deleted, they are only 'deactivated')
     - task check [id] (date)            - set task as completed for date, or today if there's no date given. You can put any date and time and the repo will recognise it, but keep it to 'YYYY-MM-DD' if possible.
     - task deactivate [ID]              - deactivates a given task (tasks are not deleted, they are only 'deactivated')
     - task list (date) (filter active)  - lists out all tasks and whether they're completed for the specified day. If no date is give, gives today. Filter: 0 or 1. 0 excludes all active tasks to show a list of deactivated tasks, 1 excludes all inactive tasks to show a list of active tasks (think of it as 'only show me the entries for which IsActive = #'). Displays all by default. Will assume first argument is filter active if cannot parse first argument as date.
     - task registry (filter active)     - returns task regisry. Filter: 0 or 1. 0 excludes all active tasks to show a list of deactivated tasks, 1 excludes all inactive tasks to show a list of active tasks (think of it as 'only show me the entries for which IsActive = #'). Displays all by default.
     - task uncheck [id] (date           - set task as uncompleted for date, or today if there's no date given. You can put any date and time and the repo will recognise it, but keep it to 'YYYY/MM/DD if possible.";

    public TaskCommand(TaskService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return $"Not Enough arguments, usage: {Usage}";
        switch (args[0])
        {
            case "add": return Add(args.Skip(1).ToList());
            case "activate": return Activate(args.Skip(1).ToList());
            case "check": return Check(args.Skip(1).ToList());
            case "deactivate": return Deactivate(args.Skip(1).ToList());
            case "list": return List(args.Skip(1).ToList());
            case "registry": return Registry(args.Skip(1).ToList());
            case "uncheck": return Uncheck(args.Skip(1).ToList());
            case "help": return LongHelp;
            default: return $"Unknown argument 'task {args[0]}', type 'help task'.";
        }
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: task add [name] (description)";
        if (args.Count == 1) return PrintEntry(_service.AddTask(args[0], null));
        return $"Added task: {PrintEntry(_service.AddTask(args[0], args[1]))}";
    }

    private string Activate(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: task activate [ID]";
        if (!int.TryParse(args[0], out int id)) return $"Cannot parse ID '{args[0]}', must be a number (integer). Usage: task activate [ID]";
        try {return $" Set active task: {PrintEntry(_service.Activate(id))}";}
        catch (Exception ex) {return $"Puppets.Commands.TaskCommand.Activate(): Error activating task: probably wrong ID or task doesn't exist. {ex.Message}";}
    }

    private string Check(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: task check [ID] (date)";
        if (!int.TryParse(args[0], out int id)) return $"Cannot parse ID '{args[0]}', must be a number (integer). Usage: task Check [ID] (date)";
        var toEditTask = _service.GetTask(id, null);
        if (args.Count == 1) 
        {
            try 
            {
                _service.SetComplete(id, DateTime.Now);
                return $"Set task '{toEditTask.Name}' completed for today ({DateTime.Now:yyyy-MM-dd}): ({(toEditTask.Completed ? "Completed" : "Uncompleted")} -> Completed)";
            }
            catch (Exception ex) {return $"Puppet.Commands.TaskCommand.Check(): Error setting task complete for today. Probably wrong ID: {ex.Message}";}
        }
        if (!DateTime.TryParse(args[1], out DateTime date)) return $"Cannot parse date '{args[1]}', try 'YYYY/MM/DD' format. Usage: task Check [ID] (date)";
        try 
        {
            _service.SetComplete(id, date);
            return $"Set task '{toEditTask.Name}' completed for {date.ToString("yyyy-MM-dd")}: ({(toEditTask.Completed? "Completed": "Uncompleted")} -> Completed)";
        }
        catch (Exception ex) {return $"Puppet.Commands.TaskCommand.Check(): Error setting task complete for {date.ToString("yyyy-MM-dd")}. Probably wrong ID: {ex.Message}";}
    }

    private string Deactivate(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: task activate [ID]";
        if (!int.TryParse(args[0], out int id)) return $"Cannot parse ID '{args[0]}', must be a number (integer). Usage: task activate [ID]";
        try {return $" Set active task: {PrintEntry(_service.Deactivate(id))}";}
        catch (Exception ex) {return $"Puppets.Commands.TaskCommand.Deactivate(): Error deactivating task: probably wrong ID or task doesn't exist. {ex.Message}";}
    }

    private string List(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return PrintList(_service.GetAll());
        if (!DateTime.TryParse(args[0], out DateTime date))
        {
            // in this case, we assume that they mean today.
            if (args[0] == "0") return PrintList(_service.GetAll(DateTime.Now, true, false));
            if (args[0] == "1") return PrintList(_service.GetAll(DateTime.Now, false, true));
            // if they got this far, they probably didn't
            return $"Could not parse Date '{args[0]}', nor was it 1 or 0. Try 'YYYY/MM/DD' format, or 'help task'";
        }
        if (args.Count == 1) return PrintList(_service.GetAll(date, false, false));
        if (args[1] == "0") return PrintList(_service.GetAll(date, true, false));
        if (args[1] == "1") return PrintList(_service.GetAll(date, false, true));
        return $"Invalid argument '{args[1]}', should be either 1 or 0.";
    }

    private string Registry(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return PrintList(_service.RegistryList(false, false));
        if (args[0] == "0") return PrintList(_service.RegistryList(true, false));
        if (args[0] == "1") return PrintList(_service.RegistryList(false, true));
        return $"Invalid argument '{args[1]}', should be either 1 or 0.";
    }

    private string Uncheck(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: task unheck [ID] (date)";
        if (!int.TryParse(args[0], out int id)) return $"Cannot parse ID '{args[0]}', must be a number (integer). Usage: task Check [ID] (date)";
        var toEditTask = _service.GetTask(id, null);
        if (args.Count == 1) 
        {
            try 
            {
                _service.SetIncomplete(id, DateTime.Now);
                return $"Set task '{toEditTask.Name}' uncompleted for today ({DateTime.Now.ToString("yyyy-MM-dd")}): ({(toEditTask.Completed? "Completed" : "Uncompleted")} -> Uncompleted)";
            }
            catch (Exception ex) {return $"Puppet.Commands.TaskCommand.Uncheck(): Error setting task complete for today. Probably wrong ID: {ex.Message}";}
        }
        if (!DateTime.TryParse(args[1], out DateTime date)) return $"Cannot parse date '{args[1]}', try 'YYYY/MM/DD' format. Usage: task Check [ID] (date)";
        try 
        {
            _service.SetIncomplete(id, date);
            return $"Set task '{toEditTask.Name}' uncompleted for {date.ToString("yyyy-MM-dd")}: ({(toEditTask.Completed? "Completed": "Uncompleted")} -> Uncompleted)";
        }
        catch (Exception ex) {return $"Puppet.Commands.TaskCommand.Uncheck(): Error setting task complete for {date.ToString("yyyy-MM-dd")}. Probably wrong ID: {ex.Message}";}
    }

    private string PrintEntry(DailyTask entry)
    {
        return $"[{entry.Id}] {(entry.Completed ? "[X]" : "[ ]")} {entry.Name}: \"{entry.Description}\"";
    }

    private string PrintList(List<DailyTask> TaskList)
    {
        string listString = "";
        foreach (var entry in TaskList)
        {
            listString = $"{listString}{PrintEntry(entry)}\n";
        }
        return listString;
    }
}
