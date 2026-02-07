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

        public ICommand RefreshSummaryCommand { get; }

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

        public MainWindowVM(CoreServices service)
        {
            Services = service;
            Logger = service.Logger;
            Settings = service.SettingsService.Settings;
            ApplyTheme();

            RefreshSummary();

            RefreshSummaryCommand = new RelayCommand(RefreshSummary);
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

        /*
        private void DefaultSummary()
        {
            Log();
            SummaryStartTime = DateTime.Now.Date;
            SummaryEndTime = DateTime.Now.Date.AddDays(1);
            CurrentSummary = Services.dataService.GetMacroSummary(SummaryStartTime, SummaryEndTime);
            Log($"SummaryStartTime='{SummaryStartTime}', SummaryEndTime={SummaryEndTime}");
            Log($"Trying to show MacroSummary: GoalName='{CurrentSummary.GoalName}', Target.Calories='{CurrentSummary.Target.Calories}'");
        } */

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
