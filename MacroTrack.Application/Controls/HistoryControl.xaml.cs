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
    /// Interaction logic for HistoryControl.xaml
    /// </summary>
    public partial class HistoryControl : ControlBase
    {
        private readonly HistoryVM _vm = new();
        /*
        public static readonly DependencyProperty RequestRefreshCommandProperty = DependencyProperty.Register(
            nameof(RequestRefreshCommand),
            typeof(ICommand),
            typeof(FoodEntryControl)
        );
        public ICommand? RequestRefreshCommand
        {
            get => (ICommand?)GetValue(RequestRefreshCommandProperty);
            set => SetValue(RequestRefreshCommandProperty, value);
        }*/

        public HistoryControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services)
        {
            base.Init(services);
            _vm.Services = Services;
            _vm.Logger = Logger;
            _vm.Populate();
        }
    }
}
