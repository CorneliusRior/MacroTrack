using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                "Task.List",
                "Lists TaskRegistry."),
            new(["Task", "Set"],
                "Task.Set <bool Complete> <int Id> (DateTime Date)",
                "Sets specified task as complete or incomplete for specified date, or todat if not specified."),
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
                "cheat"     => Cheat(head, args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public PuppetResult Add(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public PuppetResult Current(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public PuppetResult Disable(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public PuppetResult List(IReadOnlyList<string> args)
        {
            

            throw new NotImplementedException();
        }

        public PuppetResult Set(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

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

        public PuppetResult CheatGet(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public PuppetResult CheatSet(IReadOnlyList<string> args)
        {
            if (args.IsJson())
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        public sealed record AddPayload(string Name, string? Description);
        public sealed record CurrentPayload(DateTime? Date, bool? FilterActive, bool? FilterInactive);
        public sealed record DisablePayload(int Id, bool? Reenable);
        public sealed record SetPayload(bool complete, int Id, DateTime? Date);
        public sealed record CheatGetPayload(DateTime? Date);
        public sealed record CheatSetPayload(DateTime Date, bool IsCheatDay);
    }
}
