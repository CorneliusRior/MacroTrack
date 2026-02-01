using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class FoodEntryVM : ViewModelBase
    {
        public ObservableCollection<Preset> PresetList { get; } = new();
        public ObservableCollection<string> CatList { get; } = new();



        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        private string? _itemName;
        public string? ItemName
        {
            get => _itemName;
            set { _itemName = value; OnPropertyChanged(); }
        }

        
        private Preset? _selectedPreset;
        public Preset? SelectedPreset
        {
            get => _selectedPreset;
            set { _selectedPreset = value; OnPropertyChanged(); }
        }

        private double? _mult;
        public double? Mult
        {
            get => _mult;
            set { _mult = value; OnPropertyChanged(); }
        }

        private double? _cal;
        public double? Cal
        {
            get => _cal;
            set { _cal = value; OnPropertyChanged(); }
        }

        private double? _pro;
        public double? Pro
        {
            get => _pro;
            set { _pro = value; OnPropertyChanged(); }
        }

        private double? _car;
        public double? Car
        {
            get => _car;
            set { _car = value; OnPropertyChanged(); }
        }

        private double? _fat;
        public double? Fat
        {
            get => _fat;
            set { _fat = value; OnPropertyChanged(); }
        }

        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public void Clear()
        {
            ItemName = null;
            Mult = 1;
            Cal = null;
            Pro = null;
            Car = null;
            Fat = null;
            Notes = string.Empty;
        }

        public void Add()
        {
            MessageBox.Show($"This is what we have right now:\nTime: '{(Time == null ? "No time" : Time)}'\nItemName: '{(ItemName == null ? "null" : ItemName)}\nMult: x{Mult}\nCar: {(Cal == null ? "Null" : Cal)} / Car: {(Pro == null ? "Null" : Pro)} / Car: {(Car == null ? "Null" : Car)} / Car: {(Fat == null ? "Null" : Fat)}\nNotes: '{Notes}'");
        }

        public void TimeNow()
        {
            Time = DateTime.Now;
        }

        public void NewPreset()
        {
            MessageBox.Show("Not yet implemnted");
        }

        public void Populate()
        {
            if (Services == null)
            {
                Log("Null Service, returning.", LogLevel.Warning, NullServices);
                return;
            }
            Log();
            if (Time is null) Time = DateTime.Now;
            Mult = 1;
            try
            {
                List<Preset> presetList = Services.presetService.GetAll();
                PresetList.Clear();
                foreach (Preset p in presetList)
                {
                    PresetList.Add(p);
                }

                List<string> catList = Services.presetService.GetDisplayCategoryList();
                CatList.Clear();
                foreach (string c in catList)
                {
                    CatList.Add(c);
                }
            }
            catch (Exception ex)
            {
                Log("Error", LogLevel.Warning, ex);
            }
        }

        public void FilterPresetList(int selectedIndex)
        {
            if (Services == null)
            {
                Log("Null Service, returning.", LogLevel.Warning, NullServices);
                return;
            }
            Log($"SelectedIndex = {selectedIndex}");
            List<Preset> presetList = new();
            if (selectedIndex == 0) presetList = Services.presetService.GetAll();
            if (selectedIndex == 1) presetList = Services.presetService.GetAllCategory(null);
            if (selectedIndex >= 2) presetList = Services.presetService.GetAllCategory(CatList[selectedIndex]);
            PresetList.Clear();
            foreach (Preset p in presetList)
            {
                PresetList.Add(p);
            }
        }

        public void PresetSelected(Preset p)
        {
            ItemName = p.PresetName;
            Mult = 1;
            Cal = p.Calories;
            Pro = p.Protein;
            Car = p.Carbs;
            Fat = p.Fat;
        }
    }
}
