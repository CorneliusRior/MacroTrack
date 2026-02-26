using DocumentFormat.OpenXml.ExtendedProperties;
using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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

        // Just does this for the time being, change it to be a combobox:
        private GoalType _goalType = GoalType.None;
        public GoalType GoalType
        {
            get => _goalType;
            set
            {
                _goalType = value;
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
                double delta = Math.Max(value, 0) - _calories;                
                _calories = Math.Max(value, 0);
                OnPropertyChanged();
                if (delta != 0) OnCaloriesChanged(delta);
            }
        }

        private double _minCal = 1900;
        public double MinCal
        {
            get => _minCal;
            set
            {
                _minCal = Math.Min(value, Calories);
                OnPropertyChanged();
            }
        }

        private double _maxCal = 2100;
        public double MaxCal
        {
            get => _maxCal;
            set
            {
                _maxCal = Math.Max(value, Calories);
                OnPropertyChanged();
            }
        }

        private bool _minCalEnabled = false;
        public bool MinCalEnabled
        {
            get => _minCalEnabled;
            set
            {
                _minCalEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _maxCalEnabled = false;
        public bool MaxCalEnabled
        {
            get => _maxCalEnabled;
            set
            {
                _maxCalEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _updating;
        // Macros:
        public NGMacroState P { get; } 
        public NGMacroState C { get; } 
        public NGMacroState F { get; }
        
        // Macro list for collective access:
        public IReadOnlyList<NGMacroState> Macros { get; }

        public GoalNewVM()
        {
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            AddCommand = new RelayCommand(() => Add());

            P = new(NGMacro.Protein);
            C = new(NGMacro.Carbs);
            F = new(NGMacro.Fat);
            Macros = new[] { P, C, F };
            foreach (NGMacroState m in Macros) m.PropertyChanged += MacroChanged;

            // Set them at a 40%/30%/30% default:
            P.Prop = 0.4;
            C.Prop = 0.3;
            F.Prop = 0.3;
            foreach (NGMacroState m in Macros) m.AutoSetMinMax(50);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
        }

        private void OnCaloriesChanged(double delta)
        {
            // Update min & max:
            if (!MaxCalEnabled) MaxCal += delta;
            else MaxCal = Math.Max(MaxCal, Calories);
            if (!MinCalEnabled) MinCal = Math.Max(0, MinCal + delta);
            else MinCal = Math.Min(MinCal, Calories);

            // Update macros 
            _updating = true;

            // Get the locked ones
            var free = Macros.Where(m => !m.IsLocked).ToList();           

            /*// Distribution, if all are locked, distribute evenly:
            double dist = delta / (free.Count == 0 ? 3 : free.Count);
            foreach (var m in free) m.Grams += dist / m.CaloriesPerGram;*/

            // Distribute based on weight for unlocked, if all are locked, ignore that.
            if (free.Count == 0)
            {
                free = Macros.ToList();
            }
            double wSum = free.Sum(m => m.ThisCalories);
            if (wSum == 0) foreach (var m in Macros) m.Grams += (1 / free.Count) * (delta / m.CaloriesPerGram);
            else foreach (var m in Macros) m.Grams += (m.ThisCalories / wSum) * (delta / m.CaloriesPerGram);

            // Figure out present proportions:
            foreach (var m in Macros) m.Prop = m.ThisCalories / Calories;            
           
            _updating = false;
        }

        private void Normalize(NGMacroState changed)
        {
            // List of each which is locked/set
            var locked = Macros.Where(m => m.IsLocked).ToList();
            double lockedCals = locked.Sum(m => m.ThisCalories);
            if (lockedCals > Calories && changed.IsLocked)
            {
                // Presumably this state arises due to changed being too high, so get lockedCals without changed. First get the difference:
                double removable = Math.Min(lockedCals - Calories, changed.ThisCalories);
                changed.Grams -= removable / changed.CaloriesPerGram;
                lockedCals -= removable;

                if (lockedCals > Calories)                 
                {
                    // this implies that it was a problem before. Hopefully this should never happen, but in this edge case, proportional adjustment:
                    double scale = Calories / lockedCals;
                    foreach (var m in locked) m.Grams *= scale;
                    lockedCals = Calories;
                }

            }
            double fixedCals = lockedCals + (changed.IsLocked ? 0 : changed.ThisCalories);
            double remainingCals = Math.Max(0, Calories - fixedCals);

            // Adjustables:
            var adjust = Macros.Where(m => !m.IsLocked && m.Macro != changed.Macro).ToList();

            // If no adjustabes, force to fit.
            if (adjust.Count == 0)
            {
                double changedCals = Math.Max(0, Calories - lockedCals);
                changed.Grams = changedCals / changed.CaloriesPerGram;
            }
            else
            {
                // Use current props as weightsL
                double wSum = adjust.Sum(m => m.ThisCalories);
                if (wSum <= 0) foreach (var m in adjust) m.Grams = (remainingCals / adjust.Count) / m.CaloriesPerGram;
                else foreach (var m in adjust) m.Grams = (remainingCals * (m.ThisCalories / wSum)) / m.CaloriesPerGram;
            }
            
            double calError = Calories - Macros.Sum(m => m.ThisCalories);
            var errortarget = Macros.Where(m => !m.IsLocked && m.Macro != changed.Macro).OrderByDescending(m => m.Grams).FirstOrDefault() ?? Macros.Where(m => !m.IsLocked).OrderByDescending(m => m.Grams).FirstOrDefault() ?? changed;
            if (Math.Abs(calError) > 1e-6)
            {
                errortarget.Grams += calError / errortarget.CaloriesPerGram;
                errortarget.ClampGrams(Calories);
            }

            // Clamp grams then compute props:
            foreach (var m in Macros) m.ClampGrams(Calories);
            foreach (var m in Macros)
            {
                double newProp = m.ThisCalories / Calories;
                if (Math.Abs(m.Prop - newProp) > 1e-9) m.Prop = newProp;
            }

        }

        private void MacroChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_updating) return;
            var macro = (NGMacroState)sender!;
            if (e.PropertyName == nameof(NGMacroState.Grams)) UserChangedGrams(macro);
            if (e.PropertyName == nameof(NGMacroState.Prop)) UserChangedProp(macro);
        }

        private void UserChangedGrams(NGMacroState changed)
        {
            // Grams are athoritative.
            _updating = true;
            try
            {
                changed.ClampGrams(Calories);
                changed.Prop = (changed.Grams * changed.CaloriesPerGram) / Calories;
                Normalize(changed);                
            }
            finally { _updating = false; }
        }

        private void UserChangedProp(NGMacroState changed)
        {
            _updating = true;
            try
            {
                double lockedCals = Macros.Where(m => m.IsLocked).Sum(m => m.ThisCalories);
                double maxProp = (Calories <= 0) ? 0 : Math.Max(0, 1.0 - (lockedCals / Calories));
                if (changed.Prop > maxProp) changed.Prop = maxProp;

                changed.Grams = (Calories * changed.Prop) / (changed.CaloriesPerGram);
                Normalize(changed);
            }
            finally { _updating = false; }
        }

        public static double ClampProp(double n) => n < 0 ? 0 : (n > 1 ? 1 : n);

        public void Add()
        {
            if (Services == null) throw new Exception("Null Services");
            if (AppServices == null) throw new Exception("Null AppServices");
            if (string.IsNullOrEmpty(GoalName))
            {
                MessageBox.Show("Goal name is required.");
                return;
            }

            string? customType = null; // replace this.
            string? notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes;
            double? minCal = MinCalEnabled ? Math.Round(MinCal, 1) : null;
            double? maxCal = MaxCalEnabled ? Math.Round(MaxCal, 1) : null;
            double? minPro = P.MinEnabled ? Math.Round(P.Min, 1) : null;
            double? maxPro = P.MaxEnabled ? Math.Round(P.Max, 1) : null;
            double? minCar = C.MinEnabled ? Math.Round(C.Min, 1) : null;
            double? maxCar = C.MaxEnabled ? Math.Round(C.Max, 1) : null;
            double? minFat = F.MinEnabled ? Math.Round(F.Min, 1) : null;
            double? maxFat = F.MaxEnabled ? Math.Round(F.Max, 1) : null;

            Services.goalService.AddGoal(GoalName, Math.Round(Calories, 1), Math.Round(P.Grams, 1), Math.Round(C.Grams, 1), Math.Round(F.Grams, 1), GoalType, customType, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
            AppServices.AppEvents.Publish(new GoalAdded());
            RequestClose?.Invoke(true);
        }
    }
}
