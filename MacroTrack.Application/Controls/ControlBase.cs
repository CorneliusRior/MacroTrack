using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Controls
{
    public abstract class ControlBase : UserControl
    {
        public CoreServices? Services { get; set; }
        public IMTLogger? Logger { get; set; }
        public AppServices? AppServices { get; set; }

        protected List<IDisposable> _subscriptions = new();
        

        protected ControlBase()
        {
            Loaded += (_, _) =>
            {
                if (Services != null)
                {
                    Logger = Services.Logger;
                    if (Services.SettingsService.Settings.LogInitMessages) Log($"{this.GetType().Name} Initialized");
                }
            };
            Unloaded += OnUnloaded;
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            foreach (IDisposable s in _subscriptions) s.Dispose();
        }

        public virtual void Init(CoreServices services, AppServices appServices)
        {
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
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
