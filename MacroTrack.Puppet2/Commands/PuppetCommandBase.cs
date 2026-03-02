using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public abstract class PuppetCommandBase : IPuppetCommand
    {
        protected CoreServices _services { get; }
        protected IPuppetContext _context { get; }

        protected PuppetCommandBase(CoreServices services, IPuppetContext context)
        {
            _services = services;
            _context = context;
        }

        public abstract string Name { get; }
        public virtual IReadOnlyList<string> Aliases => Array.Empty<string>();

        // Attempting to allow help to reach subcommands:
        public abstract IReadOnlyList<CommandHelp> Help { get; }

        public abstract PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args);

        protected string Location([CallerMemberName] string member = "") => $"{GetType().FullName}.{member}()";

        /// <summary>
        /// Better debugging printer. Ignore all parameters except "Message", the rest fill in automatically.
        /// </summary>
        /// <param name="message"></param>
        protected static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }
    }

    public sealed record CommandHelp(
        IReadOnlyList<string> Head, 
        string Usage, string Description,
        string Example = "", 
        string LongDescription = "", 
        IReadOnlyList<string>? Aliases = null, 
        IReadOnlyList<string>? SubCommands = null)
    {
        const int PCommandSpace = -25;
        const int PHelpSpace = -175;
        const string PIndent = " ";
        /// <summary>
        /// Only for use on ShortLists.
        /// </summary>
        public static string PrintHeader(HelpListType listType) =>
            $"{"Command:", PCommandSpace}" +
            $"{listType.ToString()}:";
        public string ShortList(HelpListType listType) => 
            $"{string.Join('.', Head).Truncate(PCommandSpace), PCommandSpace}" +
            $"{listType switch {
                HelpListType.Description => Description.Truncate(PHelpSpace),
                HelpListType.Usage       => Usage.Truncate(PHelpSpace),
                HelpListType.Example     => Example.Truncate(PHelpSpace),
                HelpListType.Aliases     => Aliases is null ? "None" : $"{{ \"{string.Join($"\", \"", Aliases)}\" }}".Truncate(PHelpSpace),
                _ => ""}, PHelpSpace}";
        public string LongList() =>
            $" * {string.Join('.', Head)}:\n" +
            $"{(SubCommands is null ? "" : $"{PIndent}Subcommands: '{string.Join("', ", SubCommands)}'\n")}" +
            $"{(Aliases is null ? "" : $"{PIndent}Aliases: {{ \"{string.Join($"\", \"", Aliases)}\" }}\n")}" +
            $"{PIndent}Usage: {Usage}\n" +
            $"{(string.IsNullOrWhiteSpace(Example) ? "" : $"{PIndent}Example: {Example}\n")}" +
            $"{PIndent}{Description}" + 
            $"{(string.IsNullOrWhiteSpace(LongDescription) ? "" : $"\n{PIndent}" + LongDescription)}";
    }

    public enum HelpListType
    {
        Description,
        Usage,
        Example,
        Aliases
    }

    public static class HelpListTypeExtensions
    {
        public static HelpListType ToHelpListType(this string i) => i.ToLowerInvariant().Trim() switch
        {
            // Actual names:
            "description" => HelpListType.Description,
            "usage" => HelpListType.Usage,
            "example" => HelpListType.Example,
            "aliases" => HelpListType.Aliases,

            // Numbers:
            "0" => HelpListType.Description,
            "1" => HelpListType.Usage,
            "2" => HelpListType.Example,
            "3" => HelpListType.Aliases,

            _ => throw new ArgumentOutOfRangeException()
        };

        public static bool IsHelpListType(this string i, out HelpListType helpListType)
        {
            // bool Name = string.IsHelpList(out var helpListType)
            helpListType = HelpListType.Description; // default, you shouldn't use it.
            try { helpListType = i.ToHelpListType(); }
            catch { return false; }
            return true;
        }

        public static bool IsHelpListType(this string i)
        {
            try { i.ToHelpListType(); }
            catch { return false; }
            return true;
        }

        public static string ToString(this HelpListType t, bool plural = false) => t switch
        {
            HelpListType.Description => plural ? "Descriptions" : "Description",
            HelpListType.Usage       => plural ? "Usages" : "Usage",
            HelpListType.Example     => plural ? "Examples" : "Example",   
            HelpListType.Aliases     => "Aliases",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
