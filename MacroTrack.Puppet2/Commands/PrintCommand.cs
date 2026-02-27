using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class PrintCommand : IPuppetCommand
    {
        public string Name => "print";
        public IReadOnlyList<string> Aliases => new[] { "p", "echo" };
        public string Usage => "print <string message>";
        public string ShortHelp => "Prints text to the output.";
        public string LongHelp => "Standard print command, echos the text back";

        private readonly CoreServices _services;
        public PrintCommand(CoreServices services, IPuppetContext context) { _services = services; }

        public PuppetResult Execute(IReadOnlyList<string> args)
        {
            if (args.Count == 0) return PuppetResult.Fail("Nothing to print :(");
            return PuppetResult.Ok(string.Join(" ", args));
        }
    }
}
