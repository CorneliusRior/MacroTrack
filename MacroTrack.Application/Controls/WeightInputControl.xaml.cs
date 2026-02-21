using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
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
    /// Interaction logic for WeightEntryControl.xaml
    /// </summary>
    public partial class WeightInputControl : ControlBase
    {
        private readonly WeightInputVM _vm = new();
        public WeightInputControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _vm.OnClose();
            base.OnUnloaded(sender, e);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Log();
            _vm.Clear();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            _vm.Add();
        }

        private void buttonNow_Click(object sender, RoutedEventArgs e)
        {
            _vm.TimeNow();
        }
    }
}
