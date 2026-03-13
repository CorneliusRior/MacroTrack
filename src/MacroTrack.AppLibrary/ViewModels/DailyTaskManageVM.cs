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
    internal class DailyTaskManageVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;
        public ObservableCollection<DailyTask> TaskList { get; } = new();

        private DailyTask? _selectedTask;
        public DailyTask? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask == value) return;
                _selectedTask = value;
                OnPropertyChanged();
                UpdateSelected();
            }
        }

        // Commands:
        public ICommand CloseCommand { get; }

        public DailyTaskManageVM()
        {
            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(false));
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<TaskCompletionChanged>(_ => Populate()));
            Populate();
        }

        public void Populate()
        {
            Log();
            if (Services == null) throw new Exception("Null Services");
            List<DailyTask> taskList = Services.taskService.RegistryList();
            TaskList.Clear();
            foreach (DailyTask t in taskList) TaskList.Add(t);
        }

        public void UpdateSelected()
        {

        }
    }
}
