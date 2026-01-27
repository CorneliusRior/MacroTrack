using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Repositories
{
    public abstract class RepoBase
    {
        protected IMTLogger Logger { get; }

        protected RepoBase(CoreContext ctx)
        {
            Logger = ctx.Logger;
            Log($"{this.GetType().Name} Initialised");
        }

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }
    }
}
