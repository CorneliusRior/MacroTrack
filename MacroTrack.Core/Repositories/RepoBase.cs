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

        /// <summary>
        /// Logs name and value of variables supplied in an anonymous object
        /// Format like LogVars(new{ a, b, c } [...] )
        /// </summary>
        /// <example>
        /// <code>
        /// LogVars(new{ a, b, c }, "Variables before");
        /// </code>
        /// </example>
        /// <param name="vars">An object whose public instances are logged</param>
        /// <param name="prefix">String which proceeds the variable listing in the log entry</param>
        /// <param name="caller">Automatically supplied member name of caller, ignore.</param>
        protected void LogVars(object vars, string? prefix = null, [CallerMemberName] string caller = "")
        {
            Logger?.LogVars(this, vars, caller, prefix);
        }
    }
}
