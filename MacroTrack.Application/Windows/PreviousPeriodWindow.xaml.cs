using MacroTrack.AppLibrary.Services;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Services;
using OxyPlot.Series;
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
    /// Interaction logic for PreviousPeriodWindow.xaml
    /// </summary>
    public partial class PreviousPeriodWindow : WindowBase
    {
        private readonly PreviousPeriodWindowVM _vm;
        public TimePeriod Timep { get; set; }
        public PreviousPeriodWindow(CoreServices services, AppServices appServices, TimePeriod timePeriod) : base(services, appServices)
        {
            InitializeComponent();
            Timep = timePeriod;
            _vm = new PreviousPeriodWindowVM(services, appServices, timePeriod);
            DataContext = _vm;
            WireUpControls();
        }

        protected override void OnClosed(EventArgs e)
        {
            _vm.OnClose();
            base.OnClosed(e);
        }

        private void WireUpControls()
        {
            DailyTasks.Init(Services, AppServices);
            Summary.Init(Services, AppServices);
            DiaryView.Init(Services, AppServices);
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            _vm.Previous();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            _vm.Next();
        }

        private void WindowBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) { _vm.Previous(); e.Handled = true; }
            if (e.Key == Key.Right) { _vm.Next(); e.Handled = true; }
        }
    }
}
