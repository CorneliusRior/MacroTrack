using MacroTrack.AppLibrary.Commands;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Settings;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MacroTrack.AppLibrary.Resources;
using MacroTrack.AppLibrary.Graphs;

namespace MacroTrack.Dashboard
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public CoreServices Services;
        public AppSettings Settings;
        public IMTLogger Logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand PrintCommand { get; }
        public event Action<string>? RequestPrint;
        public ICommand OpenSettingsCommand { get; }
        public event Action? RequestOpenSettings;

        public MainWindowVM(CoreServices service)
        {
            Services = service;
            Logger = service.Logger;
            Settings = service.SettingsService.Settings;
            ApplyTheme();

            

            PrintCommand = new RelayCommand(() => RequestPrint?.Invoke(""));
            OpenSettingsCommand = new RelayCommand(() => RequestOpenSettings?.Invoke());

            
        }

        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }


        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OpenSettings()
        {
            OpenSettingsCommand.Execute(this);
        }

        public void ApplyTheme()
        {
            ThemeManager.SetCustomColors(Settings);
            ThemeManager.SetTheme(Settings.Theme);            
        }

    }
}
