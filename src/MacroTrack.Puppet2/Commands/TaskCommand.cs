using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class TaskCommand : PuppetCommandBase
    {
        public TaskCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "task";
        public override IReadOnlyList<string> Aliases => new[] { "t", "dailytasks", "dt", "checklist" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Task"],
                "Task.<subcommand: Add/Current/Disable/List/Set/Cheat>",
                "Commands for interacting with TaskCompletion, TaskLog (cheat), and TaskRegistry"),
            new(["Task", "Add"],
                "Task.Add <string Name> (string Description)",
                "Adds a new daily task."),
            new(["Task", "Current"],
                "Task.Current (DateTime Date) (bool FilterActive) (bool FilterInactive)",
                "Returns task completion status and streaks for specified date, or today if not specified."),
            new(["Task", "Disable"],
                "Task.Disable <int Id> (bool Reenable)",
                "Sets a task as Deactivated: still exists but not displayed, or reenable deactivated tasks."),
            new(["Task", "List"],
                "Task.List (bool FilterActive) (bool FilterInactive)",
                "Lists TaskRegistry."),
            new(["Task", "Set"],
                "Task.Set <bool Complete> <int Id> (DateTime Date)",
                "Sets specified task as complete or incomplete for specified date, or todat if not specified."),
            new(["Task", "History"],
                "Task.History (DateTime StartDate) (DateTime EndDate)",
                "Prints history for tasks (matrix)"),
            new(["Task", "Cheat"],
                "Task.Cheat.<subcommand Get/Set",
                "Commands for interacting with TaskLog (cheat)",
                SubCommands: new [] { "Get", "Set" }),
            new(["Task", "Cheat", "Get"],
                "Task.Cheat.Check (DateTime Date)",
                "Checks if the specified day is a cheat day."),
            new(["Task", "Cheat", "Set"],
                "Task.Cheat.Set <DateTime Date> <bool IsCheatDat>",
                "Set the specified day as a cheat day, or not a cheat day."),
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            // This doesn't seem to work but I'll do it anyway:
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(args),
                "current"   => Current(args),
                "disable"   => Disable(args),
                "list"      => List(args),
                "set"       => Set(args),
                "history"   => History(args),
                "cheat"     => Cheat(head, args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => TestAdd(args),
                "current"   => TestCurrent(args),
                "disable"   => TestDisable(args),
                "list"      => TestList(args),
                "set"       => TestSet(args),
                "cheat"     => TestCheat(head, args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };            
        }

        public PuppetResult Add(IReadOnlyList<string> args)
        {
            DailyTask entry;
            if (args.IsJson())
            {
                AddPayload pl = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.taskService.AddTask(pl.Name, pl.Description);                
            }
            else
            {
                string name = args.String(0, "Name");
                string? desc = args.StringOrNull(1, "Description");
                entry = _services.taskService.AddTask(name, desc);
            }
            return PuppetResult.Ok($"Added Task #{entry.Id}");
        }

        private PuppetResult TestAdd(IReadOnlyList<string> args)
        {
            try
            {
                AddPayload pl = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult Current(IReadOnlyList<string> args)
        {
            DateTime date;
            bool filterInactive, filterActive;
            if (args.IsJson())
            {
                CurrentPayload pl = JsonSerializer.Deserialize<CurrentPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                date = pl.Date ?? DateTime.Now;
                filterInactive = pl.FilterInactive ?? false;
                filterActive = pl.FilterActive ?? false;
            }
            else
            {
                date = args.dateTimeOr(0, "Date", DateTime.Now);
                filterInactive = args.BoolOr(1, "FilterInactive", false);
                filterActive = args.BoolOr(2, "FilterActive", false);
            }
            List<DailyTask> taskList = _services.taskService.GetAllStreaks(date, filterActive, filterInactive);
            StringBuilder sb = new();
            sb.AppendLine($"Printing Tasks as of {date.ToString("yyyy-MM-dd")}, FilterInactive='{filterInactive}', FilterActive='{filterActive}'");
            sb.AppendLine(DailyTask.PrintHeader(true, true, true));
            foreach (DailyTask dt in taskList) sb.AppendLine(dt.Print(true, true, true));
            return PuppetResult.Ok(sb.ToString());
        }

        private PuppetResult TestCurrent(IReadOnlyList<string> args)
        {
            try
            {
                CurrentPayload pl = JsonSerializer.Deserialize<CurrentPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult Disable(IReadOnlyList<string> args)
        {
            int id;
            bool reenable;
            if (args.IsJson())
            {
                DisablePayload pl = JsonSerializer.Deserialize<DisablePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                id = pl.Id;
                reenable = pl.Reenable ?? false;
            }
            else
            {
                id = args.Int(0, "Id");
                reenable = args.BoolOr(1, "Reenable", false);
            }
            bool r = _services.taskService.GetTask(id).IsActive;
            DailyTask entry = reenable ? _services.taskService.Activate(id) : _services.taskService.Deactivate(id);
            return PuppetResult.Ok($"Set Task #{entry.Id} from '{(r ? "Active" : "Inactive")}' to '{(entry.IsActive ? "Active" : "Inactive")}'.");
        }

        private PuppetResult TestDisable(IReadOnlyList<string> args)
        {
            try
            {
                DisablePayload pl = JsonSerializer.Deserialize<DisablePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult List(IReadOnlyList<string> args)
        {
            bool filterInactive, filterActive;
            if (args.IsJson())
            {
                ListPayload pl = JsonSerializer.Deserialize<ListPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                filterInactive = pl.FilterInactive ?? false;
                filterActive = pl.FilterActive ?? false;
            }
            else
            {
                filterInactive = args.BoolOr(0, "FilterInactive", false);
                filterActive = args.BoolOr(1, "FilterActive", false);
            }
            List<DailyTask> taskList = _services.taskService.GetAll(null, filterActive, filterInactive);
            StringBuilder sb = new();
            sb.AppendLine($"Printing TaskRegistry, FilterInactive='{filterInactive}', FilterActive='{filterActive}'.");
            sb.AppendLine(DailyTask.PrintHeader(true, false, false));
            foreach (DailyTask dt in taskList) sb.AppendLine(dt.Print(true, false, false));
            return PuppetResult.Ok(sb.ToString());
        }

        private PuppetResult TestList(IReadOnlyList<string> args)
        {
            try
            {
                ListPayload pl = JsonSerializer.Deserialize<ListPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult Set(IReadOnlyList<string> args)
        {
            bool complete;
            int id;
            DateTime date;
            if (args.IsJson())
            {
                SetPayload pl = JsonSerializer.Deserialize<SetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                complete = pl.Complete;
                id = pl.Id;
                date = pl.Date ?? DateTime.Now;
            }
            else
            {
                complete = args.Bool(0, "Complete");
                id = args.Int(1, "Id");
                date = args.dateTimeOr(2, "Date", DateTime.Now);
            }
            DailyTask r = _services.taskService.GetTask(id);
            DailyTask entry = complete ? _services.taskService.SetComplete(id, date) : _services.taskService.SetIncomplete(id, date);
            return PuppetResult.Ok($"Set task #{entry.Id} from '{r.Completed.Checked(false, "Complete", "Incomplete")}' to '{entry.Completed.Checked(false, "Complete", "Incomplete")}'.");
        }

        private PuppetResult TestSet(IReadOnlyList<string> args)
        {
            try
            {
                SetPayload pl = JsonSerializer.Deserialize<SetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        private PuppetResult History(IReadOnlyList<string> args)
        {
            DateTime? start = args.dateTimeOrNull(0, "Start Time");
            DateTime? end = args.dateTimeOrNull(1, "End Time");
            return PuppetResult.Ok(_services.taskService.GetHistory(start, end).Print());
            throw new NotImplementedException();
        }

        public PuppetResult Cheat(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 3) PuppetResult.FailHelp(Help[6], FailHelpType.NoSubCommand);
            return head[2].ToLowerInvariant().Trim() switch
            {
                "get" => CheatGet(args),
                "set" => CheatSet(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{string.Join('.', head)}'.")
            };
        }

        private PuppetResult TestCheat(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 3) PuppetResult.FailHelp(Help[6], FailHelpType.NoSubCommand);
            return head[2].ToLowerInvariant().Trim() switch
            {
                "get" => TestCheatGet(args),
                "set" => TestCheatSet(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{string.Join('.', head)}'.")
            };
        }

        public PuppetResult CheatGet(IReadOnlyList<string> args)
        {
            DateTime date;
            if (args.IsJson())
            {
                CheatGetPayload pl = JsonSerializer.Deserialize<CheatGetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                date = pl.Date ?? DateTime.Now;
            }
            else date = args.dateTimeOr(0, "Date", DateTime.Now);
            bool isCheatDay = _services.taskService.GetIsCheatDay(date);
            return PuppetResult.Ok($"{date.ToString("yyyy-MM-dd")} IsCheatDay='{isCheatDay}'");

            throw new NotImplementedException();
        }

        private PuppetResult TestCheatGet(IReadOnlyList<string> args)
        {
            try
            {
                CheatGetPayload pl = JsonSerializer.Deserialize<CheatGetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult CheatSet(IReadOnlyList<string> args)
        {
            DateTime date;
            bool isCheatDay;
            if (args.IsJson())
            {
                CheatSetPayload pl = JsonSerializer.Deserialize<CheatSetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                date = pl.Date;
                isCheatDay = pl.IsCheatDay;
            }
            else
            {
                date = args.dateTime(0, "Date");
                isCheatDay = args.Bool(1, "IsCheatDay");
            }
            bool r = _services.taskService.GetIsCheatDay(date);
            _services.taskService.SetCheatDay(date, isCheatDay);
            return PuppetResult.Ok($"Set {date.ToString("yyyy-MM-dd")} IsCheatDay from '{r}' to '{isCheatDay}'");
        }

        private PuppetResult TestCheatSet(IReadOnlyList<string> args)
        {
            try
            {
                CheatSetPayload pl = JsonSerializer.Deserialize<CheatSetPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        private sealed record AddPayload(string Name, string? Description);
        private sealed record CurrentPayload(DateTime? Date, bool? FilterInactive, bool? FilterActive);
        private sealed record DisablePayload(int Id, bool? Reenable);
        private sealed record SetPayload(bool Complete, int Id, DateTime? Date);
        private sealed record ListPayload(bool? FilterInactive, bool? FilterActive);
        private sealed record CheatGetPayload(DateTime? Date);
        private sealed record CheatSetPayload(DateTime Date, bool IsCheatDay);        
    }
}
