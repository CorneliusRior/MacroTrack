using MacroTrack.AppLibrary.Commands;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    internal class LoggingVM : CategoryVMBase
    {
        public CoreServices Services;
        public IMTLogger Logger;
        public ICommand ViewLogCommand { get; }
        public ICommand ViewLogDirCommand { get; }
        public LoggingVM(AppSettings settings, CoreServices services) : base("Logging", settings)
        {
            Services = services;
            Logger = services.Logger;

            ViewLogCommand = new RelayCommand(() => Logger.OpenLogFile());
            ViewLogDirCommand = new RelayCommand(() => Logger.OpenLogDir());
        }
    }
}
