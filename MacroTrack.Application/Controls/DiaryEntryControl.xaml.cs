using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Controls
{
    /// <summary>
    /// Interaction logic for DiaryEntryControl.xaml
    /// </summary>
    public partial class DiaryEntryControl : ControlBase
    {
        private readonly DiaryEntryVM _vm = new();
        public DiaryEntryControl()
        {
            InitializeComponent();
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
            _vm.OnClose();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Services == null)
            {
                var ex = new Exception("Null Service");
                Log("Could not add entry.", LogLevel.Error);
                throw ex;
            }
            Services.diaryService.AddEntry(tbDiary.Text);
            tbDiary.Clear();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Log();
            tbDiary.Clear();
        }

        private void ButtonViewDiary_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm.ShowDiaryView();
        }
    }
}
