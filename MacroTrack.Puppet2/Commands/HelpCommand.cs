using DocumentFormat.OpenXml.Drawing.Charts;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            new(["Help"], 
                "Help (command) and/or (string HelpListType)", 
                "Prints command information on all commands, or more detailed information on one and its subcommands if specified.", 
                Aliases: Aliases,
                LongDescription: "HelpListTypes include: 'Description (default)', 'Usage', 'Example', 'Aliases': Only applies to short list. If no command is specified, will print the command and description, or specified HelpListType, for all commands which Puppet has access to."),
            new(["Help", "List"],
                "Help.List (command) (string HelpListType)",
                "Prints command information on all commands in a single-line list form, or just commands specified and its subcommands.",
                LongDescription: "HelpListTypes include: 'Description (default)', 'Usage', 'Example', 'Aliases'. If no command is specified, will print every command and description, or specified HelpListType, for all commands which Puppet has access to."),
            new(["Help", "Full"],
                "Help.Full (command)",
                "Prints all available information for all commands, or for specified command and its subcommands if specified.")
            
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            /*
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
            return PuppetResult.Ok(longhelpstring);*/

            // Rewriting it here:
            var allHelp = _context.CommandList.SelectMany(c => c.Help).OrderBy(c => string.Join('.', c.Head));

            /* Possible command formats we want to be able to handle:
             *  - help                      (head.Count == 0, args.count == 0)
             *  - help food                 (head.Count == 0, args.count == 1)
             *  - help food usage           (head.Count == 0, args.count == 2)
             *  - help.list                 (head.Count == 1, args.count == 0)
             *  - help.list food            (head.Count == 1, args.count == 1)
             *  - help.list food usage      (head.Count == 1, args.count == 2)
             *  - help.full                 (head.Count == 2, args.Count == 0)
             *  - help.full food            (head.Count == 2, args.Count == 1)
             * We'll iterate through args.Count.
             */

            if (head.Count < 2)
            {
                if (args.Count == 0) return ShortList(args, allHelp);
                if (args.Count == 1)
                {
                    if (args[0].IsHelpListType()) return ShortList(args, allHelp);
                    return LongList(args, allHelp);
                }
                else return ShortList(args, allHelp);
                
            }
            return head[1].ToLowerInvariant().Trim() switch
            {
                "list"  => ShortList(args, allHelp),
                "full"  => LongList(args, allHelp),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        private PuppetResult ShortList(IReadOnlyList<string> args, IOrderedEnumerable<CommandHelp> helpList)
        {
            /* 4 possible command formats:
             *  - help
             *  - help food
             *  - help usage
             *  - help food usage
             */
            HelpListType type = HelpListType.Description;

            StringBuilder sb = new();
            if (args.Count == 0)
            {
                sb.AppendLine("Printing all Commands:");
            }
            if (args.Count == 1)
            {
                // If we can parse first argument as a HelpListType, parse it as a command:
                if (!args[0].IsHelpListType(out HelpListType t))
                { // Can't parse:
                    helpList = Search(args[0], helpList);
                    sb.AppendLine($"Printing all Commands with head '{args[0].ToLowerInvariant().Trim()}':");
                } // Can parse:
                else
                {
                    type = t;
                    sb.AppendLine($"Printing all Commands and {type.ToString(true)}:");
                }
            }
            if (args.Count > 1)
            {
                // If there are 2 or more arguments, then the first one is command, the second one is HelpListType.
                helpList = Search(args[0], helpList);
                type = args[1].ToHelpListType();
                sb.AppendLine($"Printing all Commands and {type.ToString(true)} with head '{args[0].ToLowerInvariant().Trim()}'");
            }
            //if (helpList.Count() == 0) return PuppetResult.Fail($"Unknown command '{args[0] ?? ""}'.");

            sb.AppendLine(CommandHelp.PrintHeader(type));
            foreach (CommandHelp c in helpList) sb.AppendLine(c.ShortList(type));
            sb.AppendLine("Use 'help <command>' for more details.");
            return PuppetResult.Ok(sb.ToString());
        }

        private PuppetResult LongList(IReadOnlyList<string> args, IOrderedEnumerable<CommandHelp> helpList)
        {
            p($"args={{ '{string.Join("', '", args)}' }}, helpList={{ '{string.Join("', '", helpList)}' }}");
            StringBuilder sb = new();
            if (args.Count > 0)
            {
                p($"Determined args.Count > 0. args[0]='{args[0]}'");
                helpList = Search(args[0], helpList);
                sb.AppendLine($"Printing all Commands with head '{args[0].ToLowerInvariant().Trim()}':");
            }
            else sb.AppendLine($"Printing all Commands:");
            if (helpList.Count() == 0) return PuppetResult.Fail($"Unknown command '{args[0]}'.");
            
            foreach (CommandHelp c in helpList) sb.AppendLine(Environment.NewLine + c.LongList());            
            return PuppetResult.Ok(sb.ToString());
        }

        private IOrderedEnumerable<CommandHelp> Search(string? searchString, IOrderedEnumerable<CommandHelp> allHelp)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return allHelp;
            p($"searchString='{searchString}', splitSearchString='{string.Join("', '", searchString.Split('.', StringSplitOptions.RemoveEmptyEntries))}'");
            return allHelp.Where(
                c => c.Head.HeadSearch(
                    searchString.Split('.', StringSplitOptions.RemoveEmptyEntries)
                    )
                ).OrderBy(c => string.Join('.', c.Head));
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
