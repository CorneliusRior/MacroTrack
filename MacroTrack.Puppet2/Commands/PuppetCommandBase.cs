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

    public sealed record CommandHelp(IReadOnlyList<string> Head, string Usage, string Description, string Example = "", string LongDescription = "", IReadOnlyList<string>? Aliases = null);
}
