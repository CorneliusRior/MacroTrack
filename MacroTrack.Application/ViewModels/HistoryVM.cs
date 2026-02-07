using MacroTrack.Core.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class HistoryVM : ViewModelBase
    {
        public ObservableCollection<FoodEntry> Entries { get; } = new ObservableCollection<FoodEntry>();

        public HistoryVM()
        {
            Log();
            //Populate();
        }

        public void Populate()
        {
            Log();
            if (Services == null) return;
            List<FoodEntry> HistoryList = Services.foodLogService.FromTimes(DateTime.Now.Date.AddDays(-3), DateTime.Now.Date.AddDays(3));
            foreach (var entry in HistoryList)
            {
                Entries.Add(entry);
                Log($"Entry: {entry.ItemName}");
            }

        }
    }
}
