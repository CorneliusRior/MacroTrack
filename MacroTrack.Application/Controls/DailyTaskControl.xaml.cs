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

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            nameof(Date), typeof(DateTime?), typeof(DailyTaskControl),
            new PropertyMetadata(null, OnDateChanged)
            );
        public DateTime? Date
        {
            get => (DateTime?)GetValue(DateProperty);
            set => SetValue(DateProperty, value);                
        }

        public DailyTaskControl()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            _vm.Init(services, appServices);
            _vm.Date = Date;
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
            _vm.OnClose();
        }

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {                        
            var view = (DailyTaskControl)d;
            view._vm.Date = (DateTime?)e.NewValue;
        }

        public void Refresh()
        {
            _vm.Populate();
        }

        private void buttonNewTask_Click(object sender, RoutedEventArgs e)
        {
            Log($"Date={Date}");
        }

        private void btManageTasks_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
