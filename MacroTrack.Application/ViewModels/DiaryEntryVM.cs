using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.ViewModels
{
    internal class DiaryEntryVM : ViewModelBase
    {
        public DiaryEntryVM()
        {

        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            // Add event subscriptions here (SubscribeEvent())
        }

        public void ShowDiaryView()
        {
            if (AppServices == null) return;
            AppServices.WindowService.Show(WindowType.DiaryView);
        }
    }
}
