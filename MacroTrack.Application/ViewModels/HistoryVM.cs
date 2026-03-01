using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;

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
    public class HistoryVM : ViewModelBase
    {
        public ObservableCollection<FoodEntry> Entries { get; } = new ObservableCollection<FoodEntry>();

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

        private TimePeriod? _period;
        public TimePeriod? Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteEntryCommand { get; }
        public ICommand EditEntryCommand { get; }

        //public event Action? RequestRefresh;

        public HistoryVM()
        {
            DeleteEntryCommand = new RelayCommand<FoodEntry>(DeleteEntry);
            EditEntryCommand = new RelayCommand<FoodEntry>(EditEntry);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<FoodLogChanged>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<SettingsChanged>(_ => Populate()));
            Populate();
        }

        public void Populate()
        {
            Log($"Populating, Period={(Period is null ? "Null" : Period.StartTime)}-{(Period is null ? "Null" : Period.EndTime)}");
            if (Services == null) throw new Exception("Null Services");
            List<FoodEntry> HistoryList;
            if (Period is not null)
            {
                HistoryList = Services.foodLogService.FromTimes(Period.StartTime, Period.EndTime);
            }
            else
            {
                int days = Services.SettingsService.Settings.HistoryLength;
                if (Services.SettingsService.Settings.HistoryShowFuture)
                {
                    HistoryList = Services.foodLogService.FromTimes(DateTime.Today.AddDays(-days), DateTime.Today.AddDays(days));
                }
                else
                {
                    HistoryList = Services.foodLogService.FromTimes(DateTime.Today.AddDays(-days), DateTime.Today.AddDays(1));
                }                
            }
            
            Entries.Clear();
            TimeFormat = Services.SettingsService.GetDTFormatShortString();
            foreach (var entry in HistoryList) Entries.Add(entry);
        }

        private void DeleteEntry(FoodEntry? entry)
        {
            if (Services == null) return;
            if (AppServices == null) return;
            if (entry == null)
            {
                Log($"Null entry, returning.", LogLevel.Warning);
                return;
            }
            try
            {
                MessageBoxResult response = MessageBox.Show($"Delete Food Log Entry #{entry.Id} '{entry.ItemName}'?\nThis cannot be undone.", "Delete Food Log Entry", MessageBoxButton.YesNo);
                if (response == MessageBoxResult.Yes)
                {
                    Services.foodLogService.DeleteEntry(entry.Id);
                    Entries.Remove(entry);
                    AppServices.AppEvents.Publish(new FoodLogChanged());
                }
            }
            catch (Exception ex)
            {
                Log($"Error deleting entry #{entry.Id}, returning.", LogLevel.Warning, ex);
                return;
            }            
        }

        private void EditEntry(FoodEntry? entry)
        {
            if (Services == null) throw new Exception("Null Services");
            if (AppServices == null) throw new Exception("Null AppServices");
            if (entry == null)
            {
                Log("Attempted to edit Food Log Entry, but given entry is null. Probably deleted before clicking, but history did not update.", LogLevel.Warning);
                MessageBoxResult response = MessageBox.Show($"Error: Attempted to edit Food Log Entry, but given entry is null. Probably deleted before clicking, but history did not update.\nPlease check logs.\nReturning, refresh History?", "Error editing Food Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (response == MessageBoxResult.Yes)
                {
                    AppServices.AppEvents.Publish(new FoodLogChanged());
                    return;
                }
                else return;
            }
            AppServices.WindowService.Show(WindowType.FoodLogEdit, entry);
        }
    }

    
}
