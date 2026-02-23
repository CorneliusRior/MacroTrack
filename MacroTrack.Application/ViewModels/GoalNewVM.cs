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
    internal class GoalNewVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }

        public GoalNewVM()
        {
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
            AddCommand = new RelayCommand(() => Add());
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
        }

        public void Add()
        {

        }
    }
}
