using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.DataModels;
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
    /// Interaction logic for WeightViewControl.xaml
    /// </summary>
    public partial class WeightViewControl : ControlBase
    {
        private readonly WeightViewVM _vm = new();

        public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(
            nameof(Period), typeof(TimePeriod), typeof(WeightViewControl),
            new PropertyMetadata(null, OnPeriodChanged)
        );
        public TimePeriod Period
        {
            get => (TimePeriod)GetValue(PeriodProperty);
            set => SetValue(PeriodProperty, value);
        }

        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (WeightViewControl)d;
            view.Log($"Period Changed, was {e.OldValue}, now {e.NewValue} ({(TimePeriod)e.NewValue})");
            view._vm.Period = (TimePeriod)e.NewValue;
            view._vm.Populate();
            view._vm.DrawGraph();
        }

        public WeightViewControl()
        {
            InitializeComponent();
            _vm.Period = Period;
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

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            _vm.DeleteSelected();
        }
    }
}
