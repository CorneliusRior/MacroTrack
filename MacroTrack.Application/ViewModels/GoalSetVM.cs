using MacroTrack.AppLibrary.Commands;
using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class GoalSetVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;

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
        }

        private void Delete()
        {
            
        }

        private void Set()
        {
            
        }
    }
}
