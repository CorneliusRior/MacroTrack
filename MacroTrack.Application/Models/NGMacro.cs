using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Models
{
    /// <summary>
    /// NGMacro stands for "New Goal", as it is used in that particular context and the work "Macro" is used in a lot of other palces.
    /// </summary>
    public enum NGMacro
    {
        Protein, Carbs, Fat
    }

    public static class NGMacroExtensions
    {
        public static int CalPerGram(this NGMacro macro)
        {
            return macro switch
            {
                NGMacro.Protein => 4,
                NGMacro.Carbs => 4,
                NGMacro.Fat => 9,
                _ => throw new InvalidOperationException()
            };
        }

        public static string GetNameString(this NGMacro macro)
        {
            return macro switch
            {
                NGMacro.Protein => "Protein",
                NGMacro.Carbs => "Carbs",
                NGMacro.Fat => "Fat",
                _ => throw new InvalidOperationException()
            };
        }
    }


    public sealed class NGMacroState : ObservableObject
    {
        public NGMacro Macro { get; }
        public int CaloriesPerGram { get; }
        public string Name => Macro.GetNameString();

        private bool _isLocked;
        public bool IsLocked
        {
            get => _isLocked;
            set => SetProperty(ref _isLocked, value);
        }

        private double _prop;
        public double Prop
        {
            get => _prop;
            set 
            {
                if (SetProperty(ref _prop, ClampProp(value)))
                {
                    OnPropertyChanged(nameof(DisplayProp));
                }                 
            }
        }

        private double _grams;
        public double Grams
        {
            get => _grams;
            set 
            { 
                if (SetProperty(ref _grams, Math.Max(value, 0)))
                {
                    OnPropertyChanged(nameof(DisplayGrams));
                    OnPropertyChanged(nameof(ThisCalories));
                }
            }
        }

        private double _min;
        public double Min
        {
            get => _min;
            set => SetProperty(ref _min, Math.Min(value, Grams));
        }

        private double _max;
        public double Max
        {
            get => _max;
            set => SetProperty(ref _max, Math.Max(value, Grams));
        }

        private bool _minEnabled;
        public bool MinEnabled
        {
            get => _minEnabled;
            set => SetProperty(ref _minEnabled, value);
        }

        private bool _maxEnabled;
        public bool MaxEnabled
        {
            get => _maxEnabled;
            set => SetProperty(ref _maxEnabled, value);
        }

        public double DisplayGrams => Math.Round(Grams, 1);
        public double DisplayProp => Math.Round(Prop * 100, 1); // percentage

        /// <summary>
        /// Calories of this macro on its own, CaloriesPerGram * Grams
        /// </summary>
        public double ThisCalories => CaloriesPerGram * Grams;

        public NGMacroState(NGMacro macro)
        {
            Macro = macro;
            CaloriesPerGram = macro.CalPerGram();
        }

        public static double ClampProp(double n) => n < 0 ? 0 : (n > 1 ? 1 : n);
        public void ClampGrams(double totalCal)
        {
            if (Grams < 0) Grams = 0;
            if (ThisCalories > totalCal) Grams = totalCal / CaloriesPerGram;
        }
    }
}
