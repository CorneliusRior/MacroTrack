using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Models;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private bool _multUpdating;

        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set 
            {
                if (_time == value) return;
                _time = value; 
                OnPropertyChanged();
                Log($"Time updated, nameof='{nameof(Time)}', Time={Time}");
                DateTimeRequire(nameof(Time), Time);
            }
        }

        private string? _itemName;
        public string? ItemName
        {
            get => _itemName;
            set 
            {
                if (_itemName == value) return;
                _itemName = value; 
                OnPropertyChanged();
                StringRequire(nameof(ItemName), ItemName);
            }
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
            set 
            {
                if (_mult == value) return;
                _mult = value; 
                OnPropertyChanged();

                _multUpdating = true;
                RefreshScaledValues();
                _multUpdating = false;
            }
        }

        private readonly ScaledValue _cal = new();
        public double? Cal
        {
            get => _cal.GetDisplayed(Mult);
            set
            {
                if (_multUpdating) return;
                _cal.SetFromDisplayed(value, Mult);
                OnPropertyChanged();
                ClearError(nameof(Cal));
            }
        }

        private readonly ScaledValue _pro = new();
        public double? Pro
        {
            get => _pro.GetDisplayed(Mult);
            set
            {
                if (_multUpdating) return;
                _pro.SetFromDisplayed(value, Mult);
                OnPropertyChanged();
                ClearError(nameof(Pro));
            }
        }

        private readonly ScaledValue _car = new();
        public double? Car
        {
            get => _car.GetDisplayed(Mult);
            set
            {
                if (_multUpdating) return;
                _car.SetFromDisplayed(value, Mult);
                OnPropertyChanged();
                ClearError(nameof(Car));
            }
        }

        private readonly ScaledValue _fat = new();
        public double? Fat
        {
            get => _fat.GetDisplayed(Mult);
            set
            {
                if (_multUpdating) return;
                _fat.SetFromDisplayed(value, Mult);
                OnPropertyChanged();
                ClearError(nameof(Fat));
            }
        }

        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        private void RefreshScaledValues()
        {
            OnPropertyChanged(nameof(Cal));
            OnPropertyChanged(nameof(Pro));
            OnPropertyChanged(nameof(Car));
            OnPropertyChanged(nameof(Fat));
        }

        public void Clear()
        {
            ItemName = null;
            Mult = 1;
            _cal.SetBase(null);
            _pro.SetBase(null);
            _car.SetBase(null);
            _fat.SetBase(null);
            RefreshScaledValues();
            Notes = string.Empty;

            ClearAllErrors();
        }

        public void ClearAllErrors()
        {
            ClearError(nameof(Time));
            ClearError(nameof(ItemName));
            ClearError(nameof(Cal));
            ClearError(nameof(Pro));
            ClearError(nameof(Car));
            ClearError(nameof(Fat));
        }

        public void Add()
        {
            //MessageBox.Show($"This is what we have right now:\nTime: '{(Time == null ? "No time" : Time)}'\nItemName: '{(ItemName == null ? "null" : ItemName)}\nMult: x{Mult}\nCar: {(Cal == null ? "Null" : Cal)} / Car: {(Pro == null ? "Null" : Pro)} / Car: {(Car == null ? "Null" : Car)} / Car: {(Fat == null ? "Null" : Fat)}\nNotes: '{Notes}'");

            // Ensure validity. Will make red boxes in a moment but not now:
            bool error = false;
            string errorString = "Error:";
            //if (Time == null) error = true; errorString += $"\n - Null Time";

            //if (ItemName == null) error = true; errorString += $"\n - Null Name";

            /*
            if (Cal == null) error = true; errorString += $"\n - Null Cal";
            if (Pro == null) error = true; errorString += $"\n - Null Pro";
            if (Car == null) error = true; errorString += $"\n - Null Car";
            if (Fat == null) error = true; errorString += $"\n - Null Fat";
            */

            bool ok = true;
            ok &= DateTimeRequire(nameof(Time), Time);
            ok &= StringRequire(nameof(ItemName), ItemName);
            ok &= NumericRequire(nameof(Cal), Cal);
            ok &= NumericRequire(nameof(Pro), Pro);
            ok &= NumericRequire(nameof(Car), Car);
            ok &= NumericRequire(nameof(Fat), Fat);

            // Adding this for the time being, get rid of it.
            if (!ok) error = true; errorString += $"\n - Something in macros";


            if (error)
            {
                return;
            }
            try 
            { 
                FoodEntry entry = Services?.foodLogService.AddEntry(Time.Value, ItemName, Mult.Value, Cal.Value, Pro.Value, Car.Value, Fat.Value, "none", Notes);
                Log($"Added entry #{entry.Id}", LogLevel.Info);
                Clear();
            }
            catch (Exception ex) { Log("Could not add entry", LogLevel.Error, ex); }
            
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
                Log("Null Service, returning.", LogLevel.Warning);
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
                Log("Null Service, returning.", LogLevel.Warning);
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
            ClearAllErrors();
            ItemName = p.PresetName;
            Mult = 1;
            _cal.SetBase(p.Calories);
            _pro.SetBase(p.Protein);
            _car.SetBase(p.Carbs);
            _fat.SetBase(p.Fat);
            RefreshScaledValues();
        }
    }
}
