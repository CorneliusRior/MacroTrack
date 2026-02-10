using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Models;
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
    /// Interaction logic for FoodEntryControl.xaml
    /// </summary>
    public partial class FoodEntryControl : ControlBase
    {
        private readonly FoodEntryVM _vm = new();

        public FoodEntryControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Services = Services;
            _vm.Logger = Logger;
            _vm.Populate();
        }

        public static readonly DependencyProperty RequestRefreshCommandProperty = DependencyProperty.Register(
            nameof(RequestRefreshCommand),
            typeof(ICommand),
            typeof(FoodEntryControl)
        );
        public ICommand? RequestRefreshCommand
        {
            get => (ICommand?)GetValue(RequestRefreshCommandProperty);
            set => SetValue(RequestRefreshCommandProperty, value);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            _vm.Clear();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            _vm.Add();
            RequestRefreshCommand?.Execute(null);
        }

        private void buttonNow_Click(object sender, RoutedEventArgs e)
        {
            _vm.TimeNow();
        }

        private void buttonNewPreset_Click(object sender, RoutedEventArgs e)
        {
            _vm.NewPreset();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _vm.FilterPresetList(cbFilter.SelectedIndex);
        }

        private void cbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbItem.SelectedItem is Preset p)
            {
                _vm.PresetSelected(p);
            }
        }
    }
}
