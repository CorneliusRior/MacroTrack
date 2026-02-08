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
        public ObservableCollection<string> ThemeList { get; } = new();        

        public GeneralVM(AppSettings settings) : base("General", settings)
        {
            foreach (var t in ThemeManager.GetThemeList()) ThemeList.Add(t);
        }
    }
}
