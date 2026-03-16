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
    internal class DailyTaskManageVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;
        public ObservableCollection<DailyTask> TaskList { get; } = new();        

        private bool _isEditing = false;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (IsEditing == value) return;
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        private DailyTask? _selectedTask;
        public DailyTask? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask == value) return;
                _selectedTask = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelected));
                UpdateSelected();
            }
        }

        private DailyTask? _selectedEditable;
        public DailyTask? SelectedEditable
        {
            get => _selectedEditable;
            set
            {
                if (_selectedEditable == value) return;
                _selectedEditable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelected));
            }
        }

        public bool HasSelected => SelectedTask is not null;

        // Commands:
        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand OpenEditCommand { get; }
        public ICommand OpenViewCommand { get; }

        public DailyTaskManageVM()
        {
            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            EditCommand = new RelayCommand(() => Edit());
            OpenEditCommand = new RelayCommand(() => OpenEdit());
            OpenViewCommand = new RelayCommand(() => OpenView());
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<TaskCompletionChanged>(_ => Populate()));
            Populate();
        }

        public void Populate(int? id = null)
        {
            Log();
            if (Services == null) throw new Exception("Null Services");
            List<DailyTask> taskList = Services.taskService.RegistryList();
            TaskList.Clear();
            foreach (DailyTask t in taskList) TaskList.Add(t);
            if (id is not null) SelectedTask = TaskList.FirstOrDefault(t => t.Id == id);
        }

        public void OpenEdit()
        {
            if (SelectedTask is null) return;
            SelectedEditable = SelectedTask.Clone();
            IsEditing = true;
        }

        public void OpenView()
        {
            SelectedEditable = null;
            IsEditing = false;
        }

        public void Edit()
        {
            if (SelectedEditable is null) return;
            if (Services is null) throw new Exception("Null Services.");
            Services.taskService.Edit(SelectedEditable.Id, SelectedEditable);
            Populate(SelectedEditable.Id);
            OpenView();
            if (AppServices is not null) AppServices.AppEvents.Publish(new TaskCompletionChanged());
        }

        public void UpdateSelected()
        {
            if (SelectedTask is null) return;
            if (IsEditing) SelectedEditable = SelectedTask.Clone();
        }
    }
}
