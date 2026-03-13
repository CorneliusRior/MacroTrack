using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
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
    internal class DailyTaskVM : ViewModelBase
    {
        public ObservableCollection<DailyTask> Tasks { get; } = new ObservableCollection<DailyTask>();
        
        // Make these DP/Settings:
        private bool _filterActive = false;
        private bool _filterInactive = true;


        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
                Populate();
            }
        }


        // Commands:
        public ICommand ToggleCompleteCommand { get; }
        public ICommand OpenTaskManage { get; set; }

        public DailyTaskVM()
        {
            ToggleCompleteCommand = new RelayCommand<DailyTask>(ToggleComplete);
            OpenTaskManage = new RelayCommand<bool?>(_ => AppServices!.WindowService.Show(WindowType.TaskManage));
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);   
            ApplySettings();
            EventSubscribe(AppServices!.AppEvents.Subscribe<GeneralRefresh>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<TaskCompletionChanged>(_ => Populate()));
            EventSubscribe(AppServices!.AppEvents.Subscribe<SettingsChanged>(_ =>
            {
                ApplySettings();
                Populate();
            }));
            Populate();
        }

        private void ApplySettings()
        {
            _filterActive = !Services!.SettingsService.Settings.TaskShowActive;
            _filterInactive = !Services!.SettingsService.Settings.TaskShowInactive;
        }

        public void Populate()
        {
            Log();
            if (Services == null) throw new Exception("Null Services");
            List<DailyTask> tasksRaw = Services.taskService.GetAllStreaks(Date ?? DateTime.Now, _filterActive, _filterInactive);
            Tasks.Clear();
            foreach (DailyTask t in tasksRaw) Tasks.Add(t);
        }

        public void ToggleComplete(DailyTask? task)
        {
            if (Services == null) throw new Exception("Null Services");
            if (task == null)
            {
                Log("Error trying to toggle completion status for a DailyTask. Probably, task was deleted or deactivated and list didn't update. Returning (nothing happened)", LogLevel.Warning);
                return;
            }
            int id = task.Id;
            if (task.Completed) Services.taskService.SetComplete(id, Date);
            else Services.taskService.SetIncomplete(id, Date);

            Log($"Task #{id} completion changed from '{!task.Completed}' to '{task.Completed}'", LogLevel.Info);
            AppServices?.AppEvents.Publish(new TaskCompletionChanged());
            Populate();
        }
    }
}
