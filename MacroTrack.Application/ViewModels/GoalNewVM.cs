using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Services;
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
            set
            {
                if (_protein == value) return;
                _protein = value;
                OnPropertyChanged();
            }
        }

        private double _carbs;
        public double Carbs
        {
            get => _carbs;
            set
            {
                if (_carbs == value) return;
                _carbs = value;
                OnPropertyChanged();
            }
        }

        private double _fat;
        public double Fat
        {
            get => _fat;
            set
            {
                if (_fat == value) return;
                _fat = value;
                OnPropertyChanged();
            }
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

        private double _proProp = 0.2;
        public double ProProp
        {
            get => _proProp;
            set
            {
                if (_proProp == value) return;
                _proProp = Math.Min(1, value);
                if(!_propUpdating) OnProPropChanged();
                OnPropertyChanged();
            }
        }

        private double _carProp = 0.3;
        public double CarProp
        {
            get => _carProp;
            set
            {
                if (_carProp == value) return;
                _carProp = Math.Min(1, value);
                if (!_propUpdating) OnCarPropChanged();
                OnPropertyChanged();
            }
        }

        private double _fatProp = 0.5;
        public double FatProp
        {
            get => _fatProp;
            set
            {
                if (_fatProp == value) return;
                _fatProp = Math.Min(1, value);
                if (!_propUpdating) OnFatPropChanged();
                OnPropertyChanged();
            }
        }

        private bool _propUpdating = false;

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

        // Data/Binding handling:
        private void OnCaloriesChanged(double d)
        {
            if (MaxCalEnabled) MaxCal = Math.Max(MaxCal, Calories);
            else MaxCal += d;
            if (MinCalEnabled) MinCal = Math.Min(MinCal, Calories);
            else MinCal += d;
            PropToGram();
        }

        private void OnProPropChanged()
        {
            _propUpdating = true;
            // See which of them have their values clamped. First, both:
            if (SetCarEnabled && SetFatEnabled) ProProp = 1 - CarProp - FatProp;
            else if (SetCarEnabled || CarProp == 0) FatProp = 1 - CarProp - ProProp;
            else if (SetFatEnabled || FatProp == 0) CarProp = 1 - FatProp - ProProp;
            else
            {
                // Get vacuum:
                double v = 1 - ProProp - CarProp - FatProp;
                CarProp = CarProp += v * (CarProp / (CarProp + FatProp));
                FatProp = FatProp += v * (FatProp / (CarProp + FatProp));
            }
            PropToGram();
            _propUpdating = false;
        }

        private void OnCarPropChanged()
        {
            _propUpdating = true;
            // See which of them have their values clamped. First, both:
            if (SetProEnabled && SetFatEnabled) CarProp = 1 - ProProp - FatProp;
            else if (SetProEnabled || ProProp == 0) FatProp = 1 - ProProp - CarProp;
            else if (SetFatEnabled || FatProp == 0) ProProp = 1 - FatProp - CarProp;
            else
            {
                // Get vacuum:
                double v = 1 - ProProp - CarProp - FatProp;
                ProProp = ProProp += v * (ProProp / (ProProp + FatProp));
                FatProp = FatProp += v * (FatProp / (ProProp + FatProp));
            }
            PropToGram();
            _propUpdating = false;
        }

        private void OnFatPropChanged()
        {
            _propUpdating = true;
            // See which of them have their values clamped. First, both:
            if (SetCarEnabled && SetFatEnabled) FatProp = 1 - ProProp - CarProp;
            else if (SetProEnabled || ProProp == 0) CarProp = 1 - ProProp - FatProp;
            else if (SetCarEnabled || CarProp == 0) ProProp = 1 - CarProp - FatProp;
            else
            {
                // Get vacuum:
                double v = 1 - ProProp - CarProp - FatProp;
                ProProp = ProProp += v * (ProProp / (ProProp + CarProp));
                CarProp = CarProp += v * (CarProp / (ProProp + CarProp));
            }
            PropToGram();
            _propUpdating = false;
        }

        private void PropToGram()
        {
            if (!SetProEnabled) Protein = Math.Round((Calories * ProProp) / 4, 2);
            if (!SetCarEnabled) Carbs = Math.Round((Carbs * CarProp) / 4, 2);
            if (!SetFatEnabled) Fat = Math.Round((Fat * CarProp) / 9, 2);
        }
    }
}
