using MacroTrack.AppLibrary.Models;
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
    /// Interaction logic for DiaryViewControl.xaml
    /// </summary>
    public partial class DiaryViewControl : ControlBase
    {
        private readonly DiaryViewVM _vm = new();

        public static readonly DependencyProperty ShowTopRibbonProperty = DependencyProperty.Register(
            nameof(ShowTopRibbon), typeof(bool), typeof(DiaryViewControl),
            new PropertyMetadata(true)
        );
        public bool ShowTopRibbon
        {
            get => (bool)GetValue(ShowTopRibbonProperty);
            set => SetValue(ShowTopRibbonProperty, value);
        }

        public static readonly DependencyProperty ShowViewDayProperty = DependencyProperty.Register(
            nameof(ShowViewDay), typeof(bool), typeof(DiaryViewControl),
            new PropertyMetadata(true)
        );
        public bool ShowViewDay
        {
            get => (bool)GetValue(ShowViewDayProperty);
            set => SetValue(ShowViewDayProperty, value);
        }

        public static readonly DependencyProperty PreviousPeriodProperty = DependencyProperty.Register(
            nameof(PreviousPeriod), typeof(TimePeriod), typeof(DiaryViewControl),
            new PropertyMetadata(null, OnPeriodChanged)
        );
        public TimePeriod? PreviousPeriod
        {
            get => (TimePeriod?)GetValue(PreviousPeriodProperty);
            set => SetValue(PreviousPeriodProperty, value);
        }

        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (DiaryViewControl)d;
            view._vm.PreviousPeriod = (TimePeriod?)e.NewValue;
            view._vm.Populate();
        }

        public DiaryViewControl()
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
    }
}
