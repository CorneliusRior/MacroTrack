using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public CoreServices? Services;
        public IMTLogger? Logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public Exception NullServices = new("Null Services.");
        public Exception NullLogger = new("Null Logger.");

        protected void Init(CoreServices services)
        {
            Services = services;
            Logger = services.Logger;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {            
            Logger?.Log(this, caller, level, message, ex);
            if (Logger == null) Log("Error", LogLevel.Error, NullLogger);
        }
    }
}
