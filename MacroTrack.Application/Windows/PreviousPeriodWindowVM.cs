using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows
{
    internal class PreviousPeriodWindowVM : INotifyPropertyChanged
    {
        public CoreServices Services;
        public IMTLogger Logger;
        public AppServices AppServices;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected List<IDisposable> _subscriptions = new();

        // Variables:
        private TimePeriod _period;
        public TimePeriod Period
        {
            get => _period;
            set
            {
                if (_period == value) return;
                _period = value;                
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
            }
        }

        private MacroSummary? _currentSummary;
        public MacroSummary? CurrentSummary
        {
            get => _currentSummary;
            set
            {
                if (_currentSummary == value) return;
                _currentSummary = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime => Period.StartTime;
        public DateTime EndTime => Period.EndTime;

        private string _dateString = "Thursday, 19th of February, 2026";
        public string DateString
        {
            get => _dateString;
            set
            {
                if (_dateString == value) return;
                _dateString = value;
                OnPropertyChanged();
            }
        }

        // Function:
        public PreviousPeriodWindowVM(CoreServices services, AppServices appServices, TimePeriod period)
        {
            Services = services;
            Logger = services.Logger;
            AppServices = appServices;
            _period = period;
            
            Populate();
        }

        public void Populate()
        {
            Log($"Populate called for {Period.ToString("yyyy-MM-dd HH:mm")}", LogLevel.Info);
            DateString = Services.SettingsService.TimePeriodToString(Period);
            CurrentSummary = Services.dataService.GetMacroSummary(StartTime, EndTime);
        }

        public void Previous()
        {
            Period = Period.StepBack();
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(StartTime));
            Populate();
        }

        public void Next()
        {
            Period = Period.StepForward();
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(StartTime));
            Populate();
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
    }
}
