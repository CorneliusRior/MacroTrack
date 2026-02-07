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
        }

        public virtual void Init(CoreServices services)
        {
            Services = services;
            Logger = services.Logger;
        }

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger?.Log(this, caller, level, message, ex);
        }
    }
}
