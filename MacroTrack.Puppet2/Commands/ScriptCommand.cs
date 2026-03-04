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
                "Loads script, attempts to parse, prints information but does not attempt to run.")
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) return PuppetResult.Fail("Subcommand required (this is not how we decided to do these errors but eh...");
            return head[1].ToLowerInvariant().Trim() switch
            {
                "run"       => Run(args),
                "testparse" => TestParse(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        private PuppetResult Run(IReadOnlyList<string> args)
        {
            throw new NotImplementedException();
        }

        private PuppetResult TestParse(IReadOnlyList<string> args)
        {
            //return PuppetResult.Ok($"Args count]'{args.Count}', args: '{string.Join(' ', args)}'");
            string path = args.String(0, "Path");
            //return PuppetResult.Ok(path);
            var script = ScriptParser.ParseFile(path);
            return PuppetResult.Ok(script.PrintFullInfo());
        }
    }
}
