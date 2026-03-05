using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class CommandsCommand : PuppetCommandBase
    {
        public CommandsCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "commands";
        public override IReadOnlyList<string> Aliases => new[] { "command", "commandlist", "helplist" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Commands"], "Commands", "Lists all available commands.", Aliases: Aliases)    
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            string commandString = $"All commands. Type help <CommandName> for more details.";
            foreach (string cmd in _context.CommandList.OrderBy(c => c.Name).Select(c => c.Name)) commandString += Environment.NewLine + cmd;
            return PuppetResult.Ok(commandString);
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            return PuppetResult.Ok("No Json in this command.");
        }
    }
}
