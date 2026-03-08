using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class PrintCommand : PuppetCommandBase
    {
        public PrintCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "print";
        public override IReadOnlyList<string> Aliases => new[] { "p", "echo" };

        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Print"], "Print <string message>", "Prints text to the output.", Aliases: Aliases)
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (args.Count == 0) return PuppetResult.Fail("Nothing to print :(");
            return PuppetResult.Ok(string.Join(" ", args));
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            return PuppetResult.Ok("No Json in this command.");
        }
    }
}
