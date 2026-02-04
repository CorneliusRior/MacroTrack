using MacroTrack.Core.Logging;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    internal class LoggingVM : CategoryVMBase
    {
        public LoggingVM(AppSettings settings) : base("Logging", settings)
        {
            
        }
    }
}
