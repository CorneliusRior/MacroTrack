using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
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
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : WindowBase
    {
        private SettingsWindowVM _vm;
        public SettingsWindow(CoreServices services, AppServices appServices) : base(services, appServices)
        {
            InitializeComponent();
            _vm = new SettingsWindowVM(Services, AppServices);
            DataContext = _vm;
            _vm.RequestClose += r => { Close(); };
        }

        protected override void OnClosed(EventArgs e)
        {
            _vm.OnClose();
            base.OnClosed(e);
        }

        private void ButtonRevert_Click(object sender, RoutedEventArgs e)
        {
            _vm.SetSelectedToCurrent();
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            _vm.SetSelectedToDefault();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _vm.CancelCommand.Execute(null);
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            _vm.OKCommand.Execute(null);
        }
    }
}
