using MacroTrack.AppLibrary.Resources;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    internal class GeneralVM : CategoryVMBase
    {
        IMTLogger Logger;
        public ObservableCollection<string> ThemeList { get; } = new();
        public GeneralVM(AppSettings settings, IMTLogger logger) : base("General", settings)
        {
            Logger = logger;
            foreach (var t in ThemeManager.GetThemeList()) ThemeList.Add(t);
            Log("List of themese:");
            foreach (string t in ThemeList) Log(t);
        }

        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger?.Log(this, caller, level, message, ex);
        }
    }
}
