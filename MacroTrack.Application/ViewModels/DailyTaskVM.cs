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
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class DailyTaskVM : ViewModelBase
    {
        public ObservableCollection<DailyTask> Tasks { get; } = new ObservableCollection<DailyTask>();
        
        // Make these DP:
        public bool FilterActive = false;
        public bool FilterInactive = true;

        // Commands:
        public ICommand ToggleCompleteCommand { get; }

        public DailyTaskVM()
        {
            ToggleCompleteCommand = new RelayCommand<DailyTask>(ToggleComplete);
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);           
            EventSubscribe(AppServices!.AppEvents.Subscribe<TaskCompletionChanged>(_ => Populate()));
            Populate();
        }

        public void Populate()
        {
            Log();
            if (Services == null) throw new Exception("Null Services");
            List<DailyTask> tasksRaw = Services.taskService.GetAllStreaks(DateTime.Now, FilterActive, FilterInactive);
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
            if (task.Completed) Services.taskService.SetComplete(id);
            else Services.taskService.SetIncomplete(id);

                Log($"Task #{id} completion changed from '{!task.Completed}' to '{task.Completed}'", LogLevel.Info);
            Populate();
        }
    }
}
