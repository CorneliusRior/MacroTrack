using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public interface IPuppetCommand
    {
        string Name { get; }
        IReadOnlyList<string> Aliases { get; }
        IReadOnlyList<CommandHelp> Help { get; }
        PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args);
    }
}
