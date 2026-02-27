using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.Puppet2.Commands
{
    public class HelpCommand : PuppetCommandBase
    {
        public HelpCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "help";
        public override IReadOnlyList<string> Aliases => new[] { "?", "h" };
        public override string Usage => "help (string CommandName)";
        public override string ShortHelp => "Prints command information on all command, or one specifically";
        public override string LongHelp => $"If left blank, prints a list of all commands along with their description or their \"ShortHelp\". If a command name is specified, print a more detailed explanation of the command, including the name, aliases, usage, short help and long help of the command.";
        
        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (args.Count == 0)
            {
                string helpString = "All commands. Type help <CommandName> for more details.\n";
                var lines = _context.CommandList.OrderBy(c => c.Name).Select(c => $" - {c.Name + ":", -20} {c.ShortHelp}");
                return PuppetResult.Ok(helpString + string.Join(Environment.NewLine, lines));
            }
            if (!_context.TryGetCommand(args[0], out var cmd)) return PuppetResult.Fail($"Could not find command name of alias '{args[0]}', type 'help' for full list");
            return PuppetResult.Ok($"{cmd.Name}:\nAliases: {cmd.Aliases}\nUsage: {cmd.Usage}\n{cmd.ShortHelp}\n\n{cmd.LongHelp}");
        }
    }
}
