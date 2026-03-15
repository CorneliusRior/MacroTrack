using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    internal class WindowCommand : PuppetCommandBase
    {
        IPuppetContext ctx;
        public WindowCommand(CoreServices services, IPuppetContext context) : base(services, context) 
        {
            ctx = context;
        }
        public override string Name => "window";
        public override IReadOnlyList<string> Aliases => new[] { "w", "win" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Window"],
                "Window.<subcommand>",
                "A collection of commands to testing windows."),
            new(["Window", "PresetManage"],
                "Window.PresetManage",
                "Opens the Preset Manage Window",
                Aliases: ["presetm", "pstmng", "pm"]),
            new(["Window", "Settings"],
                "Window.Settings",
                "Opens the Settings Window",
                Aliases: ["set", "setting", "config"]),
            new(["Window", "AddPreset"],
                "Window.AddPreset",
                "Opens the Add Preset Window",
                Aliases: ["addpst"]),
            new(["Window", "DiaryView"],
                "Window.DiaryView",
                "Opens the Diary View Window",
                Aliases: ["dryvw", "dv"]),
            new(["Window", "GoalNew"],
                "Window.GoalNew",
                "Opens the GoalNew Window",
                Aliases: ["glnw", "gn"]),
            new(["Window", "GoalSet"],
                "Window.GoalSet",
                "Opens the Goal Set Window",
                Aliases: ["glst", "gs"]),
            new(["Window", "PreviousPeriodSelect"],
                "Window.PreviousPeriodSelect",
                "Opens the PreviousPeriodSelect Window",
                Aliases: ["pps", "prvprdslc"]),
            new(["Window", "TaskManage"],
                "Window.TaskManage",
                "Opens the TaskManage Window",
                Aliases: ["tsk", "tskmng", "tm"]),
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) return PuppetResult.Fail("Subcommand required.");
            return head[1].ToLowerInvariant().Trim() switch
            {
                "presetmanage"  => PresetManage(args),
                "presetm"       => PresetManage(args),
                "pstmng"        => PresetManage(args),
                "pm"            => PresetManage(args),
                "set"           => Settings(args),
                "settings"      => Settings(args),
                "setting"       => Settings(args),
                "config"        => Settings(args),
                "addpreset"     => AddPreset(args),
                "addpst"        => AddPreset(args),
                "diaryview"     => DiaryView(args),
                "dryvw"         => DiaryView(args),
                "dv"            => DiaryView(args),
                "goalnew"       => GoalNew(args),
                "glnw"          => GoalNew(args),
                "gn"            => GoalNew(args),
                "goalset"       => GoalSet(args),
                "glst"          => GoalSet(args),
                "gs"            => GoalSet(args),
                "previousperiodselect" => PreviousPeriodSelect(args),
                "pps"           => PreviousPeriodSelect(args),
                "prvprdslc"     => PreviousPeriodSelect(args),
                "taskmanage"    => TaskManage(args),
                "task"          => TaskManage(args),
                "tasks"         => TaskManage(args),
                "tsk"           => TaskManage(args),
                "tskmng"        => TaskManage(args),
                "tm"            => TaskManage(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            throw new NotImplementedException();
        }

        public PuppetResult Settings(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("Settings");
            return PuppetResult.Ok("Attempting to open 'Preset Manage'");
        }

        public PuppetResult AddPreset(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("AddPreset");
            return PuppetResult.Ok("Attempting to open 'Add Preset'");
        }

        public PuppetResult DiaryView(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("DiaryView");
            return PuppetResult.Ok("Attempting to open 'Diary View'");
        }

        public PuppetResult GoalNew(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("GoalNew");
            return PuppetResult.Ok("Attempting to open 'Goal New'");
        }

        public PuppetResult GoalSet(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("GoalSet");
            return PuppetResult.Ok("Attempting to open 'Goal Set'");
        }

        public PuppetResult PresetManage(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("PresetManage");
            return PuppetResult.Ok("Attempting to open 'Preset Manage'");
        }

        public PuppetResult PreviousPeriodSelect(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("PreviousPeriodSelect");
            return PuppetResult.Ok("Attempting to open 'Previous Period Select'");
        }

        public PuppetResult TaskManage(IReadOnlyList<string> args)
        {
            ctx.OpenWindow("TaskManage");
            return PuppetResult.Ok("Attempting to open 'Task Manage'");
        }

    }
}
