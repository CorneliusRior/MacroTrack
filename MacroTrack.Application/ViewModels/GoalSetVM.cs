using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class GoalSetVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;

        public ObservableCollection<Goal> GoalList { get; } = new();

        private Goal? _selectedGoal;
        public Goal? SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                if (_selectedGoal == value) return;
                _selectedGoal = value;
                OnPropertyChanged();
                UpdateSelected();
            }
        }

        private MacroTotals? _selectedTotals;
        public MacroTotals? SelectedTotals
        {
            get => _selectedTotals;
            set
            {
                if (_selectedTotals == value) return;
                _selectedTotals = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;
        private DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
            }
        }

        // Percentage strings:
        private string _proPct = "(-%)";
        public string ProPct
        {
            get => _proPct;
            set
            {
                if (_proPct == value) return;
                _proPct = value;
                OnPropertyChanged();
            }
        }

        private string _carPct = "(-%)";
        public string CarPct
        {
            get => _carPct;
            set
            {
                if (_carPct == value) return;
                _carPct = value;
                OnPropertyChanged();
            }
        }

        private string _fatPct = "(-%)";
        public string FatPct
        {
            get => _fatPct;
            set
            {
                if (_fatPct == value) return;
                _fatPct = value;
                OnPropertyChanged();
            }
        }

        // Commands:
        public ICommand CloseCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SetCommand { get; }

        public GoalSetVM()
        {
            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            DeleteCommand = new RelayCommand(() => Delete());
            SetCommand = new RelayCommand(() => Set());
            Date = DateTime.Today;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            AppServices!.AppEvents.Subscribe<GoalAdded>(msg => Populate());
            Populate();
        }

        public void Populate()
        {
            Log();
            if (Services == null) throw new Exception("Null Services");
            List<Goal> goalList = Services.goalService.GetAllGoals();
            GoalList.Clear();
            foreach (Goal g in goalList) GoalList.Add(g);
            
        }

        public void UpdateSelected()
        {
            if (Services == null) throw new Exception("Null Services");
            if (SelectedGoal is null)
            {
                ProPct = "(-%)";
                // ...
                return;
            }
            if (Services.SettingsService.Settings.GoalPctOfCalories)
            {
                double cal = SelectedGoal.Calories;
                ProPct = $"({(SelectedGoal.Protein * 400 / cal).ToString("0.#")}%)";
                CarPct = $"({(SelectedGoal.Carbs * 400 / cal).ToString("0.#")}%)";
                FatPct = $"({(SelectedGoal.Fat * 900 / cal).ToString("0.#")}%)";
            }
            else
            {
                double total = SelectedGoal.Protein + SelectedGoal.Carbs + SelectedGoal.Fat;
                ProPct = $"({(SelectedGoal.Protein / total).ToString("0.#")}%)";
                CarPct = $"({(SelectedGoal.Carbs/ total).ToString("0.#")}%)";
                FatPct = $"({(SelectedGoal.Fat / total).ToString("0.#")}%)";
            }

            SelectedTotals = new MacroTotals(SelectedGoal.Calories, SelectedGoal.Protein, SelectedGoal.Carbs, SelectedGoal.Fat);
        }

        private void Delete()
        {
            if (SelectedGoal is null) return;
            if (Services is null) throw new Exception("Null Services");
            if (AppServices is null) throw new Exception("Null Services");
            List<GoalActivation> goalActivations = Services.goalService.GetActivationsOfGoal(SelectedGoal.Id);
            string guiltString =  $"Are you sure you want to delete Goal #{SelectedGoal.Id} '{SelectedGoal.GoalName}'? This cannot be undone. This Goal has ";
            if (goalActivations.Count == 0) guiltString += "never been used.";
            else
            {
                string fmt = Services.SettingsService.GetLongDateString();
                guiltString += $"been used {goalActivations.Count} time{(goalActivations.Count == 1 ? ":" : "s:")}\n\n";
                foreach (GoalActivation ga in goalActivations)
                {
                    guiltString += $" - {ga.ActivatedAt.ToDateTime(TimeOnly.MinValue).ToString(fmt)} to {ga.DeactivatedAt.ToDateTime(TimeOnly.MinValue).ToString(fmt)}\n";
                }
                guiltString += $"\nDeleting a goal which has been used before can cause historical data to become inaccurate. Unless there was a mistake, this is not recommended.";
            }
            MessageBoxResult result = MessageBox.Show(guiltString, "Delete Goal", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) Services.goalService.DeleteGoal(SelectedGoal.Id);
            SelectedGoal = null;
            SelectedTotals = null;
            Populate();
        }

        private void Set()
        {
            // If nothing is selected, do nothing:
            if (_selectedGoal is null) return;
            if (Services == null) throw new Exception("Null Services");
            if (AppServices == null) throw new Exception("Null AppServices");
            Services.goalService.ActivateGoal(_selectedGoal.Id, DateOnly.FromDateTime(Date));
            AppServices.AppEvents.Publish(new GoalChanged());
            RequestClose?.Invoke(true);
        }
    }
}
