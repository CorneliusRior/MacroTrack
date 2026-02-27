using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
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
        public abstract string Usage { get; }
        public abstract string ShortHelp { get; }
        public abstract string LongHelp { get; }

        public abstract PuppetResult Execute(IReadOnlyList<string> args);

        protected string Location([CallerMemberName] string member = "") => $"{GetType().FullName}.{member}()";
    }
}
