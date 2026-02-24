using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
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
                CalculatePct();
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

        public void CalculatePct()
        {
            if (Services == null) throw new Exception("Null Services");
            if (_selectedGoal is null)
            {
                ProPct = "(-%)";
                // ...
                return;
            }
            if (Services.SettingsService.Settings.GoalPctOfCalories)
            {
                double cal = _selectedGoal.Calories;
                ProPct = $"({(_selectedGoal.Protein * 400 / cal).ToString("0.#")}%)";
                CarPct = $"({(_selectedGoal.Carbs * 400 / cal).ToString("0.#")}%)";
                FatPct = $"({(_selectedGoal.Fat * 900 / cal).ToString("0.#")}%)";
            }
            else
            {
                double total = _selectedGoal.Protein + _selectedGoal.Carbs + _selectedGoal.Fat;
                ProPct = $"({(_selectedGoal.Protein / total).ToString("0.#")}%)";
                CarPct = $"({(_selectedGoal.Carbs/ total).ToString("0.#")}%)";
                FatPct = $"({(_selectedGoal.Fat / total).ToString("0.#")}%)";
            }
        }

        private void Delete()
        {
            
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
