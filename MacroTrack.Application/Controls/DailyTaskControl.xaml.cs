using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Services;
using MacroTrack.Core.Logging;

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
using MacroTrack.AppLibrary.Services;

namespace MacroTrack.AppLibrary.Controls
{
    /// <summary>
    /// Interaction logic for DailyTaskControl.xaml
    /// </summary>
    public partial class DailyTaskControl : ControlBase
    {
        private readonly DailyTaskVM _vm = new();
        public DailyTaskControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Services = services;
            _vm.Logger = services.Logger;
            _vm.AppServices = appServices;
            try { _vm.Populate(); }
            catch (Exception ex) { Log("Error populating DailyTasks.", LogLevel.Error, ex); }
        }

        public void Refresh()
        {
            _vm.Populate();
        }

        private void buttonNewTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btManageTasks_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
