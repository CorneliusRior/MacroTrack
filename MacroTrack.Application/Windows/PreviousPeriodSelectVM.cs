using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
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
using System.Windows.Input;

namespace MacroTrack.AppLibrary.Windows
{
    class PreviousPeriodSelectVM : INotifyPropertyChanged
    {
        public CoreServices Services;
        public IMTLogger Logger;
        public AppServices AppServices;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<bool>? RequestClose;
        protected List<IDisposable> _subscriptions = new();

        private PreviousPeriodMode _mode;
        public PreviousPeriodMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dtSingle;
        public DateTime DTSingle
        {
            get => _dtSingle;
            set
            {
                _dtSingle = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dtCustomStart;
        public DateTime DTCustomStart
        {
            get => _dtCustomStart;
            set
            {
                _dtCustomStart = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dtCustomEnd;
        public DateTime DTCustomEnd
        {
            get => _dtCustomEnd;
            set
            {
                _dtCustomEnd = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public PreviousPeriodSelectVM(CoreServices services, AppServices appServices, ICommand openCommand)
        {
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;

            ConfirmCommand = new RelayCommand<PreviousPeriodMode>(Confirm);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));

            DTSingle = DateTime.Today.AddDays(-1);
            DTCustomStart = DateTime.Today.AddDays(-7);
            DTCustomEnd = DateTime.Today;

            _mode = PreviousPeriodMode.SingleDay;
        }

        private void Confirm(PreviousPeriodMode mode)
        {
            int weekDiff = ((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            TimePeriod period = mode switch
            {
                PreviousPeriodMode.QuickYesterday   => new TimePeriod(DateTime.Today.AddDays(-1), TimeSpan.FromDays(1)),
                PreviousPeriodMode.QuickToday       => new TimePeriod(DateTime.Today, TimeSpan.FromDays(1)),
                PreviousPeriodMode.QuickTomorrow    => new TimePeriod(DateTime.Today.AddDays(1), TimeSpan.FromDays(1)),
                PreviousPeriodMode.QuickLastWeek    => new TimePeriod(DateTime.Today.AddDays(-(weekDiff + 7)), TimeSpan.FromDays(7)),
                PreviousPeriodMode.QuickLastMonth   => new TimePeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)),
                PreviousPeriodMode.SingleDay        => new TimePeriod(DTSingle, TimeSpan.FromDays(1)),
                PreviousPeriodMode.Custom           => new TimePeriod(DTCustomStart, DTCustomEnd),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            RequestClose?.Invoke(true);
            AppServices.AppEvents.Publish(new PreviousPeriodRequested(period));
        }

        public void DTSingleNow()
        {
            DTSingle = DateTime.Today;
        }

        public void DTCustomStartNow()
        {
            DTCustomStart = DateTime.Today;
        }

        public void DTCustomEndNow()
        {
            DTCustomEnd = DateTime.Today;
        }

        public void OnClose()
        {
            foreach (IDisposable s in _subscriptions) s.Dispose();
        }

        protected void EventSubscribe(IDisposable e)
        {
            _subscriptions.Add(e);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

        // Input validation:
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

    public enum PreviousPeriodMode
    {
        QuickYesterday,
        QuickToday,
        QuickTomorrow,
        QuickLastWeek,
        QuickLastMonth,
        SingleDay,
        Custom
    }
}
