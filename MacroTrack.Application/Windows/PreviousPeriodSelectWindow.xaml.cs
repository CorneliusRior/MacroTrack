using MacroTrack.AppLibrary.Services;
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
using System.Windows.Shapes;

namespace MacroTrack.AppLibrary.Windows
{
    /// <summary>
    /// Interaction logic for PreviousPeriodSelectWindow.xaml
    /// </summary>
    public partial class PreviousPeriodSelectWindow : WindowBase
    {
        private readonly PreviousPeriodSelectVM _vm;
        public PreviousPeriodSelectWindow(CoreServices services, AppServices appServices, ICommand openCommand) : base(services, appServices)
        {
            InitializeComponent();
            _vm = new PreviousPeriodSelectVM(services, appServices, openCommand);
            DataContext = _vm;
            _vm.RequestClose += r => Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _vm.OnClose();
            base.OnClosed(e);
        }

        private void buttonSingleDayNow_Click(object sender, RoutedEventArgs e)
        {
            _vm.DTSingleNow();
        }

        private void buttonCustomStartNow_Click(object sender, RoutedEventArgs e)
        {
            _vm.DTCustomStartNow();
        }

        private void buttonCustomEndNow_Click(object sender, RoutedEventArgs e)
        {
            _vm.DTCustomEndNow();
        }
    }
}
