using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    internal class GeneralVM : CategoryVMBase
    {
        public GeneralVM(AppSettings settings) : base("General", settings)
        {
            
        }
    }
}
