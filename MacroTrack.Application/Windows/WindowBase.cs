using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroTrack.AppLibrary.Windows
{
    /// <summary>
    /// Base for all windows, except settings maybe, I don't know if I want to make it do that or not.
    /// Be careful when making this, if this includes a viewmodel, to give it "onclosed" to unsubscribe from events.
    /// </summary>
    public abstract class WindowBase : Window
    {
        protected CoreServices Services;
        protected IMTLogger Logger;
        protected AppServices AppServices;
        protected List<IDisposable> _subscriptions = new();
        

        public WindowBase(CoreServices services, AppServices appServices)
        {
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (IDisposable s in _subscriptions) s.Dispose();
            base.OnClosed(e);
        }

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger?.Log(this, caller, level, message, ex);
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
