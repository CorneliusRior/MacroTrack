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
    public class PresetManageVM : ViewModelBase
    {
        public event Action<bool>? RequestClose;

        // Commands:
        public ICommand CloseCommand { get; }
        public PresetManageVM()
        {
            CloseCommand = new RelayCommand(() => RequestClose?.Invoke(false));
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
        }
    }
}
