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
        public override string Usage => "print <string message>";
        public override string ShortHelp => "Prints text to the output.";
        public override string LongHelp => "Standard print command, echos the text back";
       
        public override PuppetResult Execute(IReadOnlyList<string> args)
        {
            if (args.Count == 0) return PuppetResult.Fail("Nothing to print :(");
            return PuppetResult.Ok(string.Join(" ", args));
        }
    }
}
