using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Graphs;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Resources;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MacroTrack.Dashboard
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public CoreServices Services;
        //public AppSettings Settings;
        public IMTLogger Logger;
        public AppServices AppServices;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand PrintCommand { get; }
        public event Action<string>? RequestPrint;
        public ICommand OpenPreviousPeriodCommand { get; }

        // Event Subscriptions:
        private List<IDisposable> _subscriptions = new List<IDisposable>();

        // Variables:
        private MacroSummary? _currentSummary;
        public MacroSummary? CurrentSummary
        {
            get => _currentSummary;
            set
            {
                _currentSummary = value;
                OnPropertyChanged();
            }
        }

        private SummaryTimeFrame _summaryTimeFrame;
        public SummaryTimeFrame SummaryTimeFrame
        {
            get => _summaryTimeFrame;
            set
            {
                _summaryTimeFrame = value;                
                OnPropertyChanged();
                RefreshSummary();
            }
        }

        private DateTime _summaryStartTime;
        public DateTime SummaryStartTime
        {
            get => _summaryStartTime;
            set
            {
                _summaryStartTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _summaryEndTime;
        public DateTime SummaryEndTime
        {
            get => _summaryEndTime;
            set
            {
                _summaryEndTime = value;
                OnPropertyChanged();
            }
        }

        // Clock variables:
        private DateTime _currentTime;
        public DateTime CurrentTime
        { 
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private string _clockFormat = "ddddd, d MMMM, yyyy, HH:mm:ss";
        public string ClockFormat
        {
            get => _clockFormat;
            set
            {
                _clockFormat = value;
                OnPropertyChanged();
            }
        }

        private string _clockString = "Time to get a new clock";
        public string ClockString
        {
            get => _clockString;
            set
            {
                _clockString = value;
                OnPropertyChanged();
            }
        }

        // History string
        private string _historyString = "Recent Entries (3 days)";
        public string HistoryString
        {
            get => _historyString;
            set
            {
                _historyString = value;
                OnPropertyChanged();
            }
        }

        // Cheat Day Button String:

        private bool _isCheatDay;
        public string DeclareCheatString
        {
            get => _isCheatDay ? "Revoke Cheat Day" : "Declare Cheat Day";
        }

        private readonly DispatcherTimer _clockTimer;

        public MainWindowVM(CoreServices service, AppServices appServices)
        {
            Services = service;
            Logger = service.Logger;
            AppServices = appServices;
            ApplyTheme();

            RefreshSummary();

            // Commands (some might be redundant)
            //RefreshSummaryCommand = new RelayCommand(RefreshSummary);
            PrintCommand = new RelayCommand(() => RequestPrint?.Invoke(""));
            OpenPreviousPeriodCommand = new RelayCommand<TimePeriod>(OpenPreviousPeriod);

            // Event Subscriptions:
            IDisposable _subSettingsChanged = AppServices.AppEvents.Subscribe<SettingsChanged>(_ =>
            {
                SetClockFormat();
                ApplyTheme();
                SetHistoryString();
            });
            _subscriptions.Add(_subSettingsChanged);
            IDisposable _foodLogChanged = AppServices.AppEvents.Subscribe<FoodLogChanged>(_ =>
            {
                RefreshSummary();
            });
            IDisposable _subGoalChanged = AppServices.AppEvents.Subscribe<GoalChanged>(_ =>
            {
                RefreshSummary();
            });
            IDisposable _subPreviousPeriodRequested = AppServices.AppEvents.Subscribe<PreviousPeriodRequested>(msg =>
            {
                OpenPreviousPeriod(msg.period);
            });

            // Clock logic:      
            SetClockFormat();
            CurrentTime = DateTime.Now;            
            ClockString = CurrentTime.ToString(ClockFormat);
            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            _clockTimer.Tick += (s, e) =>
            {
                CurrentTime = DateTime.Now;
                ClockString = CurrentTime.ToString(ClockFormat);
            };
            _clockTimer.Start();

            SetHistoryString();
            _isCheatDay = Services.taskService.GetIsCheatDay(DateTime.Today);
        }

        public void OnClose()
        {
            foreach (IDisposable s in _subscriptions) s.Dispose();
        }

        private void SetClockFormat()
        {
            Log();

            if (Services.SettingsService.Settings.ClockInLongFormat) ClockFormat = Services.SettingsService.GetLongDateTimeString();
            else ClockFormat = Services.SettingsService.GetShortDateTimeString();
            ClockString = DateTime.Now.ToString(ClockFormat);
        }

        private void SetHistoryString()
        {
            HistoryString = $"Recent Entries ({Services.SettingsService.Settings.HistoryLength} days)";
        }


        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
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
        private void LogVars(object vars, string? prefix = null, [CallerMemberName] string caller = "")
        {
            Logger.LogVars(this, vars, caller, prefix);
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        private void RefreshSummary()
        {
            Log($"SummaryTimeFrame='{SummaryTimeFrame}'");
            switch (SummaryTimeFrame)
            {
                case SummaryTimeFrame.CalendarDay: 
                    {
                        SummaryStartTime = DateTime.Now.Date;
                        SummaryEndTime = SummaryStartTime.AddDays(1);
                        break; 
                    }
                case SummaryTimeFrame.Last24Hours: 
                    {
                        SummaryStartTime = DateTime.Now.AddDays(-1);
                        SummaryEndTime = SummaryStartTime.AddDays(1);
                        break; 
                    }
                case SummaryTimeFrame.CalendarWeek: 
                    {
                        int diff = ((int)DateTime.Now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                        SummaryStartTime = DateTime.Now.AddDays(-diff);
                        SummaryEndTime= SummaryStartTime.AddDays(7);
                        break; 
                    }
                case SummaryTimeFrame.Last7Days: 
                    {
                        SummaryStartTime = DateTime.Now.Date.AddDays(-6);
                        SummaryEndTime = SummaryStartTime.AddDays(7);
                        break; 
                    }
                case SummaryTimeFrame.CalendarMonth: 
                    {
                        SummaryStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        SummaryEndTime = SummaryEndTime.AddMonths(1);
                        break; 
                    }
                case SummaryTimeFrame.Last30Days: 
                    {
                        SummaryStartTime = DateTime.Now.AddDays(-30);
                        SummaryEndTime = SummaryStartTime.AddDays(31);
                        break; 
                    }
                default: 
                    {
                        Log($"Unknown SummaryTimeFrame '{SummaryTimeFrame}'. What probably happened is a new TimeFrame was added without updating this method. Defaulting to Calendar day", LogLevel.Error);
                        SummaryStartTime = DateTime.Now.Date;
                        SummaryEndTime = SummaryStartTime.AddDays(1);
                        break; 
                    }
            }
            CurrentSummary = Services.dataService.GetMacroSummary(SummaryStartTime, SummaryEndTime);
            AppServices.AppEvents.Publish(new SummaryChanged(CurrentSummary));
        }

        public void OpenPreviousPeriod(TimePeriod? period)
        {
            if (period is null) period = new TimePeriod(DateTime.Today, DateTime.Today.AddDays(1));
            AppServices.WindowService.Show(WindowType.PreviousPeriod, period);
        }

        // Button functionality / Windows
        public void OpenPreviousPeriodYesterday()
        {
            //AppServices?.WindowService.Show(WindowType.PreviousPeriod, new TimePeriod(DateTime.Today.AddDays(-1), DateTime.Today));
            OpenPreviousPeriod(new TimePeriod(DateTime.Today.AddDays(-1), DateTime.Today));
        }

        public void OpenPreviousPeriodSelect()
        {
            AppServices.WindowService.Show(WindowType.PreviousPeriodSelect, OpenPreviousPeriodCommand);
        }

        public void OpenGoalSet()
        {
            AppServices.WindowService.Show(WindowType.GoalSet);
        }

        public void OpenGoalNew()
        {
            AppServices.WindowService.Show(WindowType.GoalNew);
        }

        public void DeclareCheatDayToday()
        {
            Services.taskService.SetCheatDay(DateTime.Today, !_isCheatDay);
            _isCheatDay = !_isCheatDay;
            RefreshSummary();
            OnPropertyChanged(nameof(DeclareCheatString));
        }

        public void OpenSettings()
        {
            AppServices.WindowService.Show(WindowType.Settings);
        }

        public void ApplyTheme()
        {
            ThemeManager.SetCustomColors(Services.SettingsService.Settings);
            ThemeManager.SetTheme(Services.SettingsService.Settings.Theme);            
        }

    }
}
