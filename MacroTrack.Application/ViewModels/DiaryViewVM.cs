using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class DiaryViewVM : ViewModelBase
    {
        private bool _isUpdating = true;
        public ObservableCollection<DiaryEntry> Entries { get; } = new ObservableCollection<DiaryEntry>();

        private DefaultTimeFrame _timeFrame;
        public DefaultTimeFrame TimeFrame
        {
            get => _timeFrame;
            set
            {
                if (_timeFrame == value) return;
                _timeFrame = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCustomTimeFrame));
                Populate();
            }
        } 
        public bool IsCustomTimeFrame => TimeFrame == DefaultTimeFrame.Custom;


        private string _timeFormat = "yyyy/MM/dd - h:mm tt";
        public string TimeFormat
        {
            get => _timeFormat;
            set
            {
                _timeFormat = value;
                OnPropertyChanged();
            }
        }

        private DateTime _customStartTime;
        public DateTime CustomStartTime
        {
            get => _customStartTime;
            set
            {
                if (_customStartTime == value) return;
                _customStartTime = value;
                OnPropertyChanged();
                Populate();
            }
        }

        private DateTime _customEndTime;
        public DateTime CustomEndTime
        {
            get => _customEndTime;
            set
            {
                if (_customEndTime == value) return;
                _customEndTime = value;
                OnPropertyChanged();
                Populate();
            }
        }

        private TimePeriod? _previousPeriod;
        public TimePeriod? PreviousPeriod
        {
            get => _previousPeriod;
            set
            {
                if (_previousPeriod == value) return;
                _previousPeriod = value;
                OnPropertyChanged();
                Populate();
            }
        }

        // Commands:
        public ICommand ViewDayCommand { get; }
        public ICommand EditEntryCommand { get; }
        public ICommand DeleteEntryCommand { get; }

        public DiaryViewVM()
        {
            ViewDayCommand = new RelayCommand<DiaryEntry>(ViewDay);
            EditEntryCommand = new RelayCommand<DiaryEntry>(EditEntry);
            DeleteEntryCommand = new RelayCommand<DiaryEntry>(DeleteEntry);

            TimeFrame = DefaultTimeFrame.Month;
            // Just doing this for the time being before we put in custom times:
            _customEndTime = DateTime.Today.AddDays(1);
            _customStartTime = _customEndTime.AddMonths(-1);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<DiaryChanged>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<SettingsChanged>(_ => Populate()));
            _isUpdating = false;
            Populate();
        }

        public void Populate()
        {
            Log();
            if (_isUpdating) return;
            _isUpdating = true;
            if (Services == null) throw new Exception("Null Services");

            // TimeFrame Logic:
            DateTime StartTime;
            DateTime EndTime;
            if (PreviousPeriod is not null)
            {
                StartTime = PreviousPeriod.StartTime;
                EndTime = PreviousPeriod.EndTime;
            }
            else if (TimeFrame == DefaultTimeFrame.Custom)
            {
                EndTime = _customEndTime;
                StartTime = _customStartTime;
            }
            else
            {
                EndTime = DateTime.Today.AddDays(1);
                StartTime = TimeFrame.GetStartDate();
            }
            Log($"Getting diary entries, startdate: {StartTime}, endtime: {EndTime}");

            // Get list and add it w/ date formatting
            List<DiaryEntry> diary = Services.diaryService.FromTimes(StartTime, EndTime);
            Entries.Clear();
            //if (Services.SettingsService.Settings.DiaryTimeIsLongFormat) TimeFormat = Services.SettingsService.GetDTFormatLongString();
            //else TimeFormat = Services.SettingsService.GetDTFormatShortString();

            TimeFormat = Services.SettingsService.GetDateTimeString(
                Services.SettingsService.Settings.DiaryTimeIsLongFormat,
                Services.SettingsService.Settings.DiaryTimeIncludesTime,
                Services.SettingsService.Settings.DiaryTimeIncludesSeconds
            );

            foreach (var entry in diary) Entries.Add(entry);

            _isUpdating = false;
        }

        public void ViewDay(DiaryEntry? entry)
        {
            if (AppServices == null) throw new Exception();
            if (entry == null)
            {
                Log("Attempted to retrieve entry, but entry was null.", LogLevel.Warning);
                MessageBox.Show($"Error: Null entry. Was probably deleted without updating DiaryView. Please use PreviousPeriods to view day.", "Null Entry", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AppServices.AppEvents.Publish(new PreviousPeriodRequested(new TimePeriod(entry.Time.Date, TimeSpan.FromDays(1))));
        }

        public void EditEntry(DiaryEntry? entry)
        {
            if (AppServices == null) throw new Exception("Null AppServices");
            AppServices.WindowService.Show(WindowType.DiaryEdit, entry);
        }

        public void DeleteEntry(DiaryEntry? entry) 
        {
            if (Services == null) throw new Exception("Null Services");
            if (entry == null)
            {
                Log("Attempted to delete entry, but entry was null.", LogLevel.Warning);
                MessageBox.Show($"Error: Null entry. Was probably deleted without updating DiaryView, refreshing.", "Null Entry", MessageBoxButton.OK, MessageBoxImage.Error);
                Populate();
                return;
            }
            MessageBoxResult response = MessageBox.Show($"Delete diary entry #{entry.Id} ({entry.Time.ToString(TimeFormat)})?\n\nThis cannot be undone.", "Delete Diary Entry", MessageBoxButton.YesNo);
            if (response == MessageBoxResult.Yes)
            {
                try
                {
                    DiaryEntry deleted = Services.diaryService.DeleteEntry(entry.Id);
                    Log($"Deleted diary entry #{entry.Id}, populating.", LogLevel.Info);
                    Populate();
                }
                catch (Exception ex)
                {
                    Log("Error deleting entry.", LogLevel.Warning, ex);
                    MessageBox.Show($"Error deleting entry, returning:\n\n{ex.Message}", "Error Deleting Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
