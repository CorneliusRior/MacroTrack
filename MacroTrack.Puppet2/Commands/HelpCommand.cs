using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public override IReadOnlyList<CommandHelp> Help => 
        [
            new(["Help"], "Help (command)", "Prints command information on all commands, or more detailed information on one if specified.", Aliases: Aliases)    
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            var allHelp = _context.CommandList.SelectMany(c => c.Help).OrderBy(c => string.Join('.', c.Head));
            if (args.Count == 0)
            {
                string helpstring = $"All commands. Type help <command> for more details.\n";
                foreach (CommandHelp c in allHelp)
                {
                    helpstring += $"\n- {string.Join('.', c.Head) + ":", -20} {c.Description}";
                }
                return PuppetResult.Ok(helpstring);
            }
            // There is an argument, tokenize and compare:
            var searchHeads = args[0].Split('.', StringSplitOptions.RemoveEmptyEntries);
            var matches = allHelp.Where(c => c.Head.HeadSearch(searchHeads)).ToList();
            if (matches.Count == 0) return PuppetResult.Fail($"Unknown command '{string.Join('.', searchHeads)}'");
            string longhelpstring = $"Commands with head '{args[0]}':";
            foreach (CommandHelp c in matches)
            {
                longhelpstring += $"\n\n{string.Join('.', c.Head)}:";
                if (c.Aliases is not null) longhelpstring += $"\n{"Aliases:",-15} {{ \"{string.Join($"\", \"", c.Aliases)}\" }}";
                longhelpstring += $"\n{"Usage:", -15} {c.Usage}\n{"Description:", -15} {c.Description}";
                if (!string.IsNullOrWhiteSpace(c.Example)) longhelpstring += $"\n{"Example:", -15} {c.Example}";
                if (!string.IsNullOrWhiteSpace(c.LongDescription)) longhelpstring += $"\n{c.LongDescription}";
            }
            return PuppetResult.Ok(longhelpstring);
        }
    }

    public static class ListHelper
    {
        /// <summary>
        /// Returns true if every parameter in B can be found in A, includes children.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool HeadSearch(this IReadOnlyList<string> a, IReadOnlyList<string> b)
        {
            if (a.Count < b.Count) return false;
            for (int i = 0; i < a.Count && i < b.Count; i++)
            {
                if (!a[i].Equals(b[i], StringComparison.OrdinalIgnoreCase)) return false;
            }
            return true;
        }
    }
}
