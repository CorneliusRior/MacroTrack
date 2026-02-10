using MacroTrack.AppLibrary.Controls;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    public class FoodEntryVM : ViewModelBase
    {
        public ObservableCollection<Preset> PresetList { get; } = new();
        public ObservableCollection<string> CatList { get; } = new();        

        private bool _multUpdating;

        public FoodEntryVM() { }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<PresetListChanged>(_ => Populate()));
            Populate();
        }



        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set 
            {
                if (_time == value) return;
                _time = value; 
                OnPropertyChanged();
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
                if (_itemName != _selectedPreset?.PresetName) SelectedPreset = null;
                OnPropertyChanged();
                StringRequire(nameof(ItemName), ItemName);
            }
        }

        
        private Preset? _selectedPreset;
        public Preset? SelectedPreset
        {
            get => _selectedPreset;
            set 
            {
                if (_selectedPreset == value) return;
                _selectedPreset = value;
                OnPropertyChanged(); 
            }
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

            bool ok = true;
            ok &= DateTimeRequire(nameof(Time), Time);
            ok &= StringRequire(nameof(ItemName), ItemName);
            ok &= NumericRequire(nameof(Cal), Cal);
            ok &= NumericRequire(nameof(Pro), Pro);
            ok &= NumericRequire(nameof(Car), Car);
            ok &= NumericRequire(nameof(Fat), Fat);

            if (!ok) return;

            // Should be a more elegant way of doing this: Could like, plug it into the require ones or something maybe? Could do that some other time, this should work for now thoughL
            DateTime time = Time!.Value;
            double mult = Mult!.Value;
            double cal = Cal!.Value;
            double pro = Pro!.Value;
            double car = Car!.Value;
            double fat = Fat!.Value;

            try 
            {
                if (Services == null) throw new Exception("Null Services");
                if (AppServices == null) throw new Exception("Null AppServices");
                FoodEntry entry = Services.foodLogService.AddEntry(time, ItemName!, mult, cal, pro, car, fat, (_selectedPreset == null ? null : _selectedPreset.PresetName), Notes);
                Log($"Added entry #{entry.Id}", LogLevel.Info);
                AppServices.AppEvents.Publish(new FoodLogChanged());
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
            if (AppServices == null) return;
            AppServices.WindowService.Show(WindowType.AddPreset);
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
            //Mult = 1; getting rid of this so we can update when presets change without mult changing.
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

        public void PresetSelected(Preset? p)
        {
            if (p != null)
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
}
