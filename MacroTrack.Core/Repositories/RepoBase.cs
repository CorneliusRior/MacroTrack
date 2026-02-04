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
    /// <summary>
    /// Base Repository, on which all repositories are based, includes logging functionality and initial '[name] initialized' log.
    /// </summary>
    public abstract class RepoBase
    {
        protected IMTLogger Logger { get; }

        protected RepoBase(CoreContext ctx)
        {
            Logger = ctx.Logger;
            if (ctx.Settings.Settings.LogInitMessages) Log($"{this.GetType().Name} Initialized");
        }

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }
    }
}
