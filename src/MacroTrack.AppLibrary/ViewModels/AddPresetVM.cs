using MacroTrack.Core.Models;
using MacroTrack.Core.Logging;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Services;
using System.Windows.Input;
using MacroTrack.AppLibrary.Commands;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class AddPresetVM : ViewModelBase
    {
        public ObservableCollection<string> CatList { get; } = new();

        // Binding Parameters:
        private string? _presetName;
        public string? PresetName
        {
            get => _presetName;
            set 
            {
                if (_presetName == value) return;
                _presetName = value;
                OnPropertyChanged();
                StringRequire(nameof(PresetName), PresetName);
            }
        }

        private double? _weight;
        public double? Weight
        {
            get => _weight;
            set
            {
                if (value == _weight) return;
                _weight = value;
                OnPropertyChanged();
            }
        }

        public bool _unitGram;
        public bool UnitGram
        {
            get => _unitGram;
            set
            {
                if (_unitGram == value) return;
                _unitGram = value;
                OnPropertyChanged();
            }
        }

        private bool _unitMl;
        public bool UnitMl
        {
            get => _unitMl;
            set
            {
                if (_unitMl == value) return;
                _unitMl = value;
                OnPropertyChanged();
            }
        }

        private string? _category;
        public string? Category
        {
            get => _category;
            set
            {
                if (_category == value) return;
                if (string.IsNullOrWhiteSpace(value)) _category = null;
                else _category = value;
                OnPropertyChanged();
            }
        }

        private double? _cal;
        public double? Cal
        {
            get => _cal;
            set
            {
                if (_cal == value) return;
                _cal = value;
                OnPropertyChanged();
                NumericRequire(nameof(Cal), Cal);
            }
        }

        private double? _pro;
        public double? Pro
        {
            get => _pro;
            set
            {
                if (_pro == value) return;
                _pro = value;
                OnPropertyChanged();
                NumericRequire(nameof(Pro), Pro);
            }
        }

        private double? _car;
        public double? Car
        {
            get => _car;
            set
            {
                if (_car == value) return;
                _car = value;
                OnPropertyChanged();
                NumericRequire(nameof(Car), Car);
            }
        }

        private double? _fat;
        public double? Fat
        {
            get => _fat;
            set
            {
                if (_fat == value) return;
                _fat = value;
                OnPropertyChanged();
                NumericRequire(nameof(Fat), Fat);
            }
        }

        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set
            {
                if (_notes == value) return;
                _notes = value;
                OnPropertyChanged();
            }
        }

        // Commands & init:
        public ICommand OpenManageCommand { get; }
        public AddPresetVM()
        {
            OpenManageCommand = new RelayCommand(() => AppServices?.WindowService.Show(WindowType.PresetManage));
        }


        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<PresetListChanged>(_ => Populate()));
            UnitGram = true; UnitMl = false;
            Populate();            
        }

        // Functions:

        public void Populate()
        {
            if (Services == null) return;
            try
            {
                List<string> catList = Services.presetService.GetCategoryList();
                CatList.Clear();
                foreach (string c in catList) CatList.Add(c);
            }
            catch (Exception ex) { Log("Error", LogLevel.Warning, ex); }
        }

        public bool Add()
        {
            Log();
            bool ok = true;
            ok &= StringRequire(nameof(PresetName), PresetName);
            ok &= NumericRequire(nameof(Cal), Cal);
            ok &= NumericRequire(nameof(Pro), Pro);
            ok &= NumericRequire(nameof(Car), Car);
            ok &= NumericRequire(nameof(Fat), Fat);
            if (!ok)
            {
                Log("One of more invalid fotmats", LogLevel.Info);
                return false;
            }
            
            double cal = Cal!.Value;
            double pro = Pro!.Value;
            double car = Car!.Value;
            double fat = Fat!.Value;

            try
            {
                if (Services == null)
                {
                    Log("Null Services", LogLevel.Error);
                    throw new Exception("Null Services");
                }
                if (AppServices == null)
                {
                    Log("Null AppServices", LogLevel.Error);
                    throw new Exception("Null AppServices");
                }
                // We're just going to keep weight and unit null for the time being.

                string? unit = null;
                if (Weight != null)
                {
                    if (UnitGram) unit = "g";
                    if (UnitMl) unit = "ml";
                }

                Preset entry = (Services.presetService.AddEntry(PresetName!, cal, pro, car, fat, Weight, unit, Category, Notes));
                Log($"Added entry #{entry.Id}", LogLevel.Info);
                AppServices.AppEvents.Publish(new PresetListChanged());
                Clear();
                return true;
            }
            catch (Exception ex) { Log("Could not add preset", LogLevel.Error, ex); return false; }
        }

        public void Clear()
        {
            PresetName = null;
            Weight = null;
            Cal = null;
            Pro = null;
            Car = null;
            Fat = null;
            Notes = null;
            ClearAllErrors();
        }

        public void ClearAllErrors()
        {
            ClearError(nameof(PresetName));
            ClearError(nameof(Cal));
            ClearError(nameof(Pro));
            ClearError(nameof(Car));
            ClearError(nameof(Fat));
        }

    }
}
