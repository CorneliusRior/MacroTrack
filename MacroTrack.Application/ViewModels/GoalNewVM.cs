using DocumentFormat.OpenXml.ExtendedProperties;
using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class GoalNewVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }

        // Variables:

        private string _goalName = "";
        public string GoalName
        {
            get => _goalName;
            set
            {
                _goalName = value;
                OnPropertyChanged();
            }
        }

        private string _notes = "";
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }


        private double _calories = 2000;
        public double Calories
        {
            get => _calories;
            set
            {
                if (_calories == value) return;
                double n = value - _calories;
                _calories = value;
                OnCaloriesChanged(n);
                OnPropertyChanged();
            }
        }

        private double _protein;
        public double Protein
        {
            get => _protein;
            set => SetGram(ref _protein, value, nameof(Protein));
        }

        private double _carbs;
        public double Carbs
        {
            get => _carbs;
            set => SetGram(ref _carbs, value, nameof(Carbs));
        }

        private double _fat;
        public double Fat
        {
            get => _fat;
            set => SetGram(ref _fat, value, nameof(Fat));
        }

        private double _minCal = 1900;
        public double MinCal
        {
            get => _minCal;
            set
            {
                if (_minCal == value) return;
                _minCal = value;
                OnPropertyChanged();
            }
        }
        
        private double _maxCal = 2100;
        public double MaxCal
        {
            get => _maxCal;
            set
            {
                if (_maxCal == value) return;
                _maxCal = value;
                OnPropertyChanged();
            }
        }
        
        private bool _minCalEnabled;
        public bool MinCalEnabled
        {
            get => _minCalEnabled;
            set
            {
                if (_minCalEnabled == value) return;
                _minCalEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _maxCalEnabled;
        public bool MaxCalEnabled
        {
            get => _maxCalEnabled;
            set
            {
                if (_maxCalEnabled == value) return;
                _maxCalEnabled = value;
                OnPropertyChanged();
            }
        }

        private double _minPro;
        public double MinPro
        {
            get => _minPro;
            set
            {
                if (_minPro == value) return;
                _minPro = value;
                OnPropertyChanged();
            }
        }

        private double _maxPro;
        public double MaxPro
        {
            get => _maxPro;
            set
            {
                if (_maxPro == value) return;
                _maxPro = value;
                OnPropertyChanged();
            }
        }

        private bool _minProEnabled;
        public bool MinProEnabled
        {
            get => _minProEnabled;
            set
            {
                if (_minProEnabled == value) return;
                _minProEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _setProEnabled;
        public bool SetProEnabled
        {
            get => _setProEnabled;
            set
            {
                if (_setProEnabled == value) return;
                _setProEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _maxProEnabled;
        public bool MaxProEnabled
        {
            get => _maxProEnabled;
            set
            {
                if (_maxProEnabled == value) return;
                _maxProEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSetPro));
                OnPropertyChanged(nameof(CanSetCar));
                OnPropertyChanged(nameof(CanSetFat));
            }
        }

        private double _minCar;
        public double MinCar
        {
            get => _minCar;
            set
            {
                if (_minCar == value) return;
                _minCar = value;
                OnPropertyChanged();
            }
        }

        private bool _setCarEnabled;
        public bool SetCarEnabled
        {
            get => _setCarEnabled;
            set
            {
                if (_setCarEnabled == value) return;
                _setCarEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSetPro));
                OnPropertyChanged(nameof(CanSetCar));
                OnPropertyChanged(nameof(CanSetFat));
            }
        }

        private double _maxCar;
        public double MaxCar
        {
            get => _maxCar;
            set
            {
                if (_maxCar == value) return;
                _maxCar = value;
                OnPropertyChanged();
            }
        }

        private bool _minCarEnabled;
        public bool MinCarEnabled
        {
            get => _minCarEnabled;
            set
            {
                if (_minCarEnabled == value) return;
                _minCarEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSetPro));
                OnPropertyChanged(nameof(CanSetCar));
                OnPropertyChanged(nameof(CanSetFat));
            }
        }

        private bool _setFatEnabled;
        public bool SetFatEnabled
        {
            get => _setFatEnabled;
            set
            {
                if (_setFatEnabled == value) return;
                _setFatEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _maxCarEnabled;
        public bool MaxCarEnabled
        {
            get => _maxCarEnabled;
            set
            {
                if (_maxCarEnabled == value) return;
                _maxCarEnabled = value;
                OnPropertyChanged();
            }
        }

        private double _minFat;
        public double MinFat
        {
            get => _minFat;
            set
            {
                if (_minFat == value) return;
                _minFat = value;
                OnPropertyChanged();
            }
        }

        private double _maxFat;
        public double MaxFat
        {
            get => _maxFat;
            set
            {
                if (_maxFat == value) return;
                _maxFat = value;
                OnPropertyChanged();
            }
        }

        private bool _minFatEnabled;
        public bool MinFatEnabled
        {
            get => _minFatEnabled;
            set
            {
                if (_minFatEnabled == value) return;
                _minFatEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _maxFatEnabled;
        public bool MaxFatEnabled
        {
            get => _maxFatEnabled;
            set
            {
                if (_maxFatEnabled == value) return;
                _maxFatEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _lockCount => (SetProEnabled ? 1 : 0) + (SetCarEnabled ? 1 : 0) + (SetFatEnabled ? 1 : 0);
        public bool CanSetPro => SetProEnabled || _lockCount < 2;
        public bool CanSetCar => SetCarEnabled || _lockCount < 2;
        public bool CanSetFat => SetFatEnabled || _lockCount < 2;

        private double _proProp = 0.2;
        public double ProProp
        {
            get => _proProp;
            set => SetProp(ref _proProp, value, nameof(ProProp));
        }

        private double _carProp = 0.3;
        public double CarProp
        {
            get => _carProp;
            set => SetProp(ref _carProp, value, nameof(CarProp));
        }

        private double _fatProp = 0.5;
        public double FatProp
        {
            get => _fatProp;
            set => SetProp(ref _fatProp, value, nameof(FatProp));
        }

        private bool _propUpdating = false;
        private bool _gramUpdating = false;

        public GoalNewVM()
        {
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            AddCommand = new RelayCommand(() => Add());
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            OnCaloriesChanged(0);
        }

        public void Add()
        {

        }

        // Proportion handling &c.:
        private void OnCaloriesChanged(double d) 
        { 
            if (MaxCalEnabled) MaxCal = Math.Max(MaxCal, Calories); 
            else MaxCal += d; 
            if (MinCalEnabled) MinCal = Math.Min(MinCal, Calories); 
            else MinCal += d; PropToGram(); 
        }

        private void SetProp(ref double backing, double value, string changedName)
        {
            value = Clamp01(value);
            if (Math.Abs(backing - value) < 1e-9) return;
            backing = value;
            OnPropertyChanged(changedName);
            if (_propUpdating) return;
            _propUpdating = true;
            try
            {
                NormalizeProps(changedName);
                PropToGram();
            }
            finally
            {
                _propUpdating = false;
                Notes = $"Calories='{Calories} / {_calories}', Protein='{Math.Round(Protein, 4)} / {Math.Round(Protein, 4)}', Carbs='{Math.Round(Carbs, 4)} / {Math.Round(_carbs, 4)}', Fat='{Math.Round(Fat, 4)} / {Math.Round(_fat, 4)}', Proptions P/C/F='{Math.Round(ProProp, 4)}/{Math.Round(CarProp, 4)}/{Math.Round(FatProp, 4)}', CalCum={Math.Round((Protein * 4) + (Carbs * 4) + (Fat * 9), 2)}, propsum='{Math.Round(ProProp + CarProp + FatProp, 4)}'";
            }
        }

        private void NormalizeProps(string changedName)
        {
            // Set up variables:
            bool lockP = SetProEnabled, lockC = SetCarEnabled, lockF = SetFatEnabled;
            double p = Clamp01(ProProp), c = Clamp01(CarProp), f = Clamp01(FatProp);

            // Changed: (which one user is changing)
            bool changedP = changedName == nameof(ProProp);
            bool changedC = changedName == nameof(CarProp);
            bool changedF = changedName == nameof(FatProp);

            // Adjustable: (neither locked nor being changed)
            bool adjP = !lockP && !changedP;
            bool adjC = !lockC && !changedC;
            bool adjF = !lockF && !changedF;

            // Weights: 
            double wP = adjP ? p : 0;
            double wC = adjC ? c : 0;
            double wF = adjF ? f : 0;
            double wSum = wP + wC + wF;

            // Locked Sum & Free Space:
            double lockedSum = (lockP ? p : 0) + (lockC ? c : 0) + (lockF ? f : 0);
            double free = Clamp01(1.0 - lockedSum);

            // FixedSum: Total we cannot change (locked & user-changed)
            double fixedSum = lockedSum + (!lockP && changedP ? p : 0)
                                        + (!lockC && changedC ? c : 0)
                                        + (!lockF && changedF ? f : 0);
            
            // Vacuum: (everything we can and must change)
            double v = Clamp01(1.0 - fixedSum);
            
            // Adjustment:
            if (adjP || adjC || adjF)
            {
                if (wSum < 1e-9)
                {
                    // If adjustables are 0 (or close enough), adjust evenly:
                    int n = (adjP ? 1 : 0) + (adjC ? 1 : 0) + (adjF ? 1 : 0);
                    if (adjP) p = v / n;
                    if (adjC) c = v / n;
                    if (adjF) f = v / n;
                }
                else
                {
                    // Otherwise distribute proprtional to current value:
                    if (adjP) p = v * (wP / wSum);
                    if (adjC) c = v * (wC / wSum);
                    if (adjF) f = v * (wF / wSum);
                }
            }
            else
            {
                // None are adjustable, enforce lock (hopefully this shouldn't happen, hence log):
                Log("User attempted to adjust with no adjustable macros: This shouldn't be possible!", LogLevel.Error);
                if (changedP && lockP) p = free;
                if (changedC && lockC) c = free;
                if (changedF && lockF) f = free;
            }

            if (Math.Abs(_proProp - p) > 1e-9) { _proProp = p; OnPropertyChanged(nameof(ProProp)); }
            if (Math.Abs(_carProp - c) > 1e-9) { _carProp = c; OnPropertyChanged(nameof(CarProp)); }
            if (Math.Abs(_fatProp - f) > 1e-9) { _fatProp = f; OnPropertyChanged(nameof(FatProp)); }

            
        }

        private void PropToGram() 
        { 
            if (!SetProEnabled) _protein = Math.Round((Calories * ProProp) / 4, 2); 
            if (!SetCarEnabled) _carbs = Math.Round((Calories * CarProp) / 4, 2); 
            if (!SetFatEnabled) _fat = Math.Round((Calories * FatProp) / 9, 2);
            OnPropertyChanged(nameof(Protein));
            OnPropertyChanged(nameof(Carbs));
            OnPropertyChanged(nameof(Fat));
        }

        private void SetGram(ref double backing, double value, string changedName)
        {
            if (changedName == nameof(Fat)) value = ClampF(value);
            else value = ClampPC(value);
            if (_gramUpdating) return;
            _gramUpdating = true;
            backing = value;
            try
            {
                if (Math.Abs(backing - value) < 1e-9) return;
                OnPropertyChanged(changedName);

                if (changedName == nameof(Protein)) ProProp = (value * 4) / Calories;
                if (changedName == nameof(Carbs)) CarProp = (value * 4) / Calories;
                if (changedName == nameof(Fat)) FatProp = (value * 9) / Calories;
                PropToGram();
            }
            finally 
            {
                _gramUpdating = false;
            };
        }

        private void GramToProp()
        {
            if (_propUpdating) return;
            _propUpdating = true;



            _propUpdating = false;
        }

        private static double Clamp01(double n) => n < 0 ? 0 : (n > 1 ? 1 : n);

        private double ClampPC(double n) => n < 0 ? 0 : (n * 4 > Calories ? Calories / 4 : n);
        private double ClampF(double n) => n < 0 ? 0 : (n * 9 > Calories ? Calories / 9 : n);
    }
}
