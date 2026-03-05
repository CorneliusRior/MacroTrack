using MacroTrack.Core.Services;
using MacroTrack.Puppet2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2
{
    public sealed class ReplService
    {
        private readonly PuppetEngine _engine;
        public ReplService(CoreServices services, IProgress<ScriptProgress>? prog)
        {
            _engine = new PuppetEngine(services, prog);
        }
        public PuppetResult Execute(string input)
        {
            return _engine.Execute(input);
        }
    }
}
