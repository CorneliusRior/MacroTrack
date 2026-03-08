using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class FoodLogEditVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;

        // I think we are supposed to give dtp format but I don't care rn.
        private int _id;

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
                // Could have a line here seeing if this is in a "Preset list" for comparison
                OnPropertyChanged();
                StringRequire(nameof(ItemName), ItemName);
            }
        }

        // We're not using this as an input, but we will keep it in the code in case we want to, and we also need this for the edit afterwards.
        private string? _categoryText;
        public string? CategoryText
        {
            get => _categoryText;
            set
            {
                if (_categoryText == value) return;
                _categoryText = value;
                OnPropertyChanged();
            }
        }

        // If you're going to keep track of presets, do it here.
        private bool _multUpdating = false;

        private double? _mult;
        public double? Mult
        {
            get => _mult;
            set
            {
                if (value == _mult) return;
                _mult = value;
                OnPropertyChanged();
                
                _multUpdating = true; 
                RefreshScaledValues(); 
                _multUpdating = false;
            }
        }

        // ScaledValues:
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
            set
            {
                if (_notes == value) return;
                _notes = value;
                OnPropertyChanged();
            }
        }        

        public void Populate(FoodEntry e)
        {
            _id = e.Id;
            ItemName = e.ItemName;
            Time = e.Time;
            CategoryText = e.Category;
            Notes = e.Notes;

            // Handling for if e.Amount == 0
            if (e.Amount == 0)
            {
                MessageBoxResult response = MessageBox.Show("Entry with amount = 0, set Mult as 1?", "Zero Amount", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (response == MessageBoxResult.Yes) Mult = 1;
                if (response == MessageBoxResult.Cancel) RequestClose?.Invoke(false);
                if (response == MessageBoxResult.No) Mult = 0;
            }
            else Mult = e.Amount;
            if (Mult == 0) // Don't actually think this is possible but you never know. Input doesn't seem to allow it.
            {
                Cal = 0;
                Pro = 0;
                Car = 0;
                Fat = 0;
            }
            else
            {
                Cal = e.Calories;
                Pro = e.Protein;
                Car = e.Carbs;
                Fat = e.Fat;
            }
        }

        public void Edit()
        {
            bool ok = true;
            ok &= DateTimeRequire(nameof(Time), Time);
            ok &= StringRequire(nameof(ItemName), ItemName);
            ok &= NumericRequire(nameof(Cal), Cal);
            ok &= NumericRequire(nameof(Pro), Pro);
            ok &= NumericRequire(nameof(Car), Car);
            ok &= NumericRequire(nameof(Fat), Fat);
            if (!ok) return;
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
                FoodEntry entry = Services.foodLogService.EditEntry(_id, time, ItemName!, mult, cal, pro, car, fat, CategoryText, Notes);
                Log($"Edited entry #{entry.Id}", LogLevel.Info);
                AppServices.AppEvents.Publish(new FoodLogChanged());
                RequestClose?.Invoke(true);
            }
            catch (Exception ex) 
            { 
                Log("Could not edit entry", LogLevel.Warning, ex);
                MessageBox.Show($"Error editing entry, please check logs. Entry probably doesn't exist (deleted while editing?)\nException: '{ex.Message}'");
            }
        }

        private void RefreshScaledValues()
        {
            OnPropertyChanged(nameof(Cal));
            OnPropertyChanged(nameof(Pro));
            OnPropertyChanged(nameof(Car));
            OnPropertyChanged(nameof(Fat));
        }
    }
}
