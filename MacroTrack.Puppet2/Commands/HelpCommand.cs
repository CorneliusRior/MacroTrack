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
            var allHelp = _context.CommandList.SelectMany(c => c.Help).OrderBy(c => string.Join('.', c.Head));
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
        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            return PuppetResult.Ok("No Json in this command.");
        }


        private PuppetResult ShortList(IReadOnlyList<string> args, IOrderedEnumerable<CommandHelp> helpList)
        {
            HelpListType type = HelpListType.Description;
            StringBuilder sb = new();

            if (args.Count == 0)
            {
                sb.AppendLine("Printing all Commands:");
            }
            if (args.Count == 1)
            { // If we can parse first argument as a HelpListType, parse it as a command:                
                if (!args[0].IsHelpListType(out HelpListType t))
                { // Can't parse:
                    helpList = Search(args[0], helpList);
                    sb.AppendLine($"Printing all Commands with head '{args[0].ToLowerInvariant().Trim()}':");
                } 
                else
                { // Can parse:
                    type = t;
                    sb.AppendLine($"Printing all Commands and {type.ToString(true)}:");
                }
            }
            if (args.Count > 1)
            { // If there are 2 or more arguments, then the first one is command, the second one is HelpListType.
                helpList = Search(args[0], helpList);
                type = args[1].ToHelpListType();
                sb.AppendLine($"Printing all Commands and {type.ToString(true)} with head '{args[0].ToLowerInvariant().Trim()}'");
            }
            if (helpList.Count() == 0) return PuppetResult.Fail($"Unknown command '{args[0] ?? ""}'.");

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

    /*
     * Pasting this little TestJson template script here for you:
     
private PuppetResult Test(IReadOnlyList<string> args)
{
    try
    {
        TestPayload pl = JsonSerializer.Deserialize<TestPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
    }
    catch { return PuppetResult.Fail("Invalid JSON payload."); }
    return PuppetResult.Ok("Parsed.");
}

PuppetResult.Ok("No Json in this command."),
    */
}
