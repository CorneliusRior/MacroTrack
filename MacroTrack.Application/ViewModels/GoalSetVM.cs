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

        private void Delete()
        {
            
        }

        private void Set()
        {
            
        }
    }
}
