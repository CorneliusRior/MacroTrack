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
        string Usage { get; }
        string ShortHelp { get; }
        string LongHelp { get; }
        PuppetResult Execute(IReadOnlyList<string> args);
    }
}
