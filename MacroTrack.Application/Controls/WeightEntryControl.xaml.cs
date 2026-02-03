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
    public partial class WeightEntryControl : ControlBase
    {
        private readonly WeightEntryVM _vm = new();
        public WeightEntryControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services)
        {
            base.Init(services);
            _vm.Services = Services;
            _vm.Logger = Logger;
            _vm.TimeNow();
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
