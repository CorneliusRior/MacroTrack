using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class CommandsCommand : IPuppetCommand
    {
        public string Name => "commands";
        public IReadOnlyList<string> Aliases => new[] { "command", "commandlist", "helplist" };
        public string Usage => "commands";
        public string ShortHelp => "Lists all commands";
        public string LongHelp => "Lists all commands which are available to the ReplEngine.";

        private readonly CoreServices _services;
        private readonly IPuppetContext _context;
        public CommandsCommand(CoreServices services, IPuppetContext context)
        {
            _services = services;
            _context = context;
        }

        public PuppetResult Execute(IReadOnlyList<string> args)
        {
            string commandString = $"All commands. Type help <CommandName> for more details.";
            foreach (string cmd in _context.CommandList.OrderBy(c => c.Name).Select(c => c.Name)) commandString += Environment.NewLine + cmd;
            return PuppetResult.Ok(commandString);
        }
    }
}
