using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public CoreServices? Services;
        public IMTLogger? Logger;
        public event PropertyChangedEventHandler? PropertyChanged;


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
        }

        // Error handling:
        protected readonly Dictionary<string, List<string>> _errors = new();
        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName == null) return null!;
            return _errors.TryGetValue(propertyName, out var list) ? list : null!;
        }

        protected void SetError(string propertyName, string error)
        {
            _errors[propertyName] = new List<string> { error };
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearError(string propertyName)
        {
            if (_errors.Remove(propertyName)) ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool NumericRequire(string propName, double? value, string message = "Cannot parse as double: Required")
        {
            if (value is null)
            {
                SetError(propName, message + ": is null"); 
                return false;
            }
            ClearError(propName);
            return true;
        }

        public bool StringRequire(string propName, string? value, string message = "Required")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SetError(propName, message);
                return false;
            }
            ClearError(propName);
            return true;
        }

        public bool DateTimeRequire(string propName, DateTime? value, string message = "Required")
        {
            if (value is null)
            {
                SetError(propName, message + ": is null");
                return false;
            }
            if (value == DateTime.MinValue)
            {
                SetError(propName, message + ": it MinValue");
                return false;
            }
            ClearError(propName);
            return true;

        }
    }
}
