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
using MacroTrack.Core.DataModels;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Controls;
using System.Windows.Threading;

namespace MacroTrack.Dashboard
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public CoreServices Services;
        //public AppSettings Settings;
        public IMTLogger Logger;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand PrintCommand { get; }
        public event Action<string>? RequestPrint;
        public ICommand OpenSettingsCommand { get; }
        public event Action? RequestOpenSettings;

        public ICommand RefreshSummaryCommand { get; }

        public Action? RequestRefreshAll;

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

        private string _clockFormat;
        public string ClockFormat
        {
            get => _clockFormat;
            set
            {
                _clockFormat = value;
                OnPropertyChanged();
            }
        }

        private string _clockString;
        public string ClockString
        {
            get => _clockString;
            set
            {
                _clockString = value;
                OnPropertyChanged();
            }
        }

        //private bool _updatingClock = false;

        private readonly DispatcherTimer _clockTimer;

        public MainWindowVM(CoreServices service)
        {
            Services = service;
            Logger = service.Logger;
            //Settings = service.SettingsService.Settings;
            ApplyTheme();

            RefreshSummary();

            RefreshSummaryCommand = new RelayCommand(RefreshSummary);
            PrintCommand = new RelayCommand(() => RequestPrint?.Invoke(""));
            OpenSettingsCommand = new RelayCommand(() => RequestOpenSettings?.Invoke());

            // Clock logic:            
            ClockFormat = "ddddd, d MMMM, yyyy, HH:MM:SS";
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
        }

        private void SetClockFormat()
        {
            Log();
            if (Services.SettingsService.Settings.ClockInLongFormat)
            {
                LogVars(new{ Services.SettingsService.Settings.ClockInLongFormat }, "Determined ClockInLongFormat=true: ");
                Log($"This is what you would get with DTFormatLList...: '{(DTFormatLList.FormatByValue.TryGetValue(Services.SettingsService.Settings.DTFormatLong, out var fnt) ? fnt : "null")}'");
                ClockFormat = DTFormatLList.FormatByValue.TryGetValue(Services.SettingsService.Settings.DTFormatLong, out var fmt) ? fmt : "ddddd, d MMMM, yyyy, HH:MM:ss";
                LogVars(new { Services.SettingsService.Settings.ClockInLongFormat }, "Meant to have changed it now.: ");
            }
            else
            {
                LogVars(new { Services.SettingsService.Settings.ClockInLongFormat, ClockFormat }, "Determined ClockInLongFormat=false: ");
                ClockFormat = DTFormatSList.FormatByValue.TryGetValue(Services.SettingsService.Settings.DTFormatShort, out var fmt) ? fmt : "yyyy/MM/dd HH:mm:ss";
                LogVars(new { Services.SettingsService.Settings.ClockInLongFormat, ClockFormat }, "Meant to have changed it now.: ");
            }
        }

        // Log & REPL handling:
        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

        private void LogVars(object vars, string? prefix = null, [CallerMemberName] string caller = "")
        {
            Logger.LogVars(this, vars, caller, prefix);
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void RequestMainRefresh()
        {
            RequestRefreshAll?.Invoke();
        }

        // If we want some parameter in settings to change something in here, put it here:
        public void RefreshVM()
        {
            SetClockFormat();
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
            RequestMainRefresh(); // shuold be the other way around maybe?
        }

        public void OpenSettings()
        {
            OpenSettingsCommand.Execute(this);
        }

        public void ApplyTheme()
        {
            ThemeManager.SetCustomColors(Services.SettingsService.Settings);
            ThemeManager.SetTheme(Services.SettingsService.Settings.Theme);            
        }

    }
}
