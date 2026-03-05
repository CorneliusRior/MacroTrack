using MacroTrack.Core.Services;
using MacroTrack.Puppet2.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public  class ScriptCommand : PuppetCommandBase
    {
        public ScriptCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "script";
        public override IReadOnlyList<string> Aliases => new[] { "mtscript" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Script"],
                "Script.<subcommand: >",
                "Commands for interacting with .mt scripts, import, testing, running, &c."),
            new(["Script", "Run"],
                "Script.Run <string Path>",
                "Runs script."),
            new(["Script", "TestParse"],
                "Script.TestParse <string Path>",
                "Loads script, attempts to parse, prints information but does not attempt to run."),
            new(["Script", "TestFormat"],
                "Script.TestFormat <string Path>",
                "Loads scripts, runs it through ScriptValidateFormat() (through TestJson()) to ensure it's in correct format.")
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) return PuppetResult.Fail("Subcommand required (this is not how we decided to do these errors but eh...");
            return head[1].ToLowerInvariant().Trim() switch
            {
                "run"       => Run(args),
                "testparse" => TestParse(args),
                "testformat"=> TestFormat(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            // There is no point in having any Json here, and that would be bad, sounds like the start of a virus or something idk.            
            return PuppetResult.Ok("No Json in this command.");
        }

        private PuppetResult Run(IReadOnlyList<string> args)
        {
            string path = args.String(0, "Path");

            Task.Run(() =>
            {
                try
                {
                    _context.RunScriptFromPath(path);
                }
                catch
                {
                    // suffer in silence
                }
            });

            return PuppetResult.Ok("");
        }

        private PuppetResult TestParse(IReadOnlyList<string> args)
        {
            //return PuppetResult.Ok($"Args count]'{args.Count}', args: '{string.Join(' ', args)}'");
            string path = args.String(0, "Path");
            //return PuppetResult.Ok(path);
            var script = ScriptParser.ParseFile(path);
            return PuppetResult.Ok(script.PrintFullInfo());
        }

        private PuppetResult TestFormat(IReadOnlyList<string> args)
        {
            string path = args.String(0, "Path");
            //var script = ScriptParser.ParseFile(path);
            //bool parsed = _context.ScriptValidateFormat(script);
            bool parsed = _context.ScriptValidateFormatPath(path);
            return PuppetResult.Bool(parsed);
        }
    }
}
