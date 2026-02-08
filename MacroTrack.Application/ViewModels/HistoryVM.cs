using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Controls;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
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

        public ICommand DeleteEntryCommand { get; }
        public ICommand EditEntryCommand { get; }

        public event Action? RequestRefresh;

        public HistoryVM()
        {
            DeleteEntryCommand = new RelayCommand<FoodEntry>(DeleteEntry);
            EditEntryCommand = new RelayCommand<FoodEntry>(EditEntry);
        }

        public void Populate()
        {
            Log();
            if (Services == null) return;
            List<FoodEntry> HistoryList = Services.foodLogService.FromTimes(DateTime.Now.Date.AddDays(-3), DateTime.Now.Date.AddDays(3));
            Entries.Clear();
            TimeFormat = Services.SettingsService.DateTimeFormatString();
            foreach (var entry in HistoryList)
            {
                Entries.Add(entry);
            }

        }

        private void DeleteEntry(FoodEntry? entry)
        {
            if (Services == null) return;
            if (entry == null)
            {
                Log($"Null entry, returning.", LogLevel.Warning);
                return;
            }
            try
            {
                Services.foodLogService.DeleteEntry(entry.Id);
                Entries.Remove(entry);
                RequestRefresh?.Invoke();
            }
            catch (Exception ex)
            {
                Log($"Error deleting entry #{entry.Id}, returning.", LogLevel.Warning, ex);
                return;
            }            
        }

        private void EditEntry(FoodEntry? entry)
        {
            MessageBox.Show("Not yet implemented");
            // basically call EditFoodEntry then Populate();
            RequestRefresh?.Invoke();
        }
    }

    
}
