using MacroTrack.AppLibrary.Models;
using MacroTrack.AppLibrary.Services;
using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
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
    /// Interaction logic for Summary.xaml
    /// </summary>
    public partial class Summary : ControlBase
    {
        public SummaryVM Vm { get; } = new SummaryVM();

        //Variables:
        public static readonly DependencyProperty CurrentSummaryProperty = DependencyProperty.Register(
            nameof(CurrentSummary),
            typeof(MacroSummary),
            typeof(Summary),
            new PropertyMetadata(null, OnCurrentSummaryChanged)
        );
        public MacroSummary? CurrentSummary
        {
            get => (MacroSummary?)GetValue(CurrentSummaryProperty);
            set => SetValue(CurrentSummaryProperty, value);
        }

        public static readonly DependencyProperty ShowTimeFrameCBProperty = DependencyProperty.Register(
            nameof(ShowTimeFrameCB), typeof(bool), typeof(Summary),
            new PropertyMetadata(true)
        );
        public bool ShowTimeFrameCB
        {
            get => (bool)GetValue(ShowTimeFrameCBProperty);
            set => SetValue(ShowTimeFrameCBProperty, value);
        }

        public static readonly DependencyProperty CenterCurrentPeriodProperty = DependencyProperty.Register(
            nameof(CenterCurrentPeriod), typeof(bool), typeof(Summary),
            new PropertyMetadata(false)
        );
        public bool CenterCurrentPeriod
        {
            get => (bool)GetValue(CenterCurrentPeriodProperty);
            set => SetValue(CenterCurrentPeriodProperty, value);
        }

        private static void OnCurrentSummaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (Summary)d;
            view.Vm.CurrentSummary = (MacroSummary?)e.NewValue;
        }

        public static readonly DependencyProperty TimeFrameProperty = DependencyProperty.Register(
            nameof(TimeFrame),
            typeof(SummaryTimeFrame),
            typeof(Summary),
            new FrameworkPropertyMetadata(
                SummaryTimeFrame.CalendarDay,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            )
        );
        public SummaryTimeFrame TimeFrame
        {
            get => (SummaryTimeFrame)GetValue(TimeFrameProperty);
            set => SetValue(TimeFrameProperty, value);
        }

        public static readonly DependencyProperty PreviousPeriodProperty = DependencyProperty.Register(
            nameof(PreviousPeriod), typeof(TimePeriod), typeof(Summary),
            new PropertyMetadata(null, OnPeriodChanged)
        );
        public TimePeriod? PreviousPeriod
        {
            get => (TimePeriod?)GetValue(PreviousPeriodProperty);
            set => SetValue(PreviousPeriodProperty, value);
        }

        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (Summary)d;
            view.Vm.PreviousPeriod = (TimePeriod?)e.NewValue;
            view.Vm.DrawGraph();
        }

        // Constructor & logic:

        public Summary()
        {
            InitializeComponent();
        }

        public override void Init(CoreServices services, AppServices appServices)
        {
            base.Init(services, appServices);
            Vm.CurrentSummary = CurrentSummary;
            //DataContext = _vm;
            Vm.Init(services, appServices);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
            Vm.OnClose();
        }
        public void Populate()
        {
            
        }
        /*
        public void Populate()
        {
            Log("We got here, print CurrentSummary things");
            LogVars(new {CurrentSummary, CurrentSummary?.GoalName, CurrentSummary?.Target.Calories});
            if (CurrentSummary == null)
            {
                CalPct = "(-%)";
                ProPct = "(-%)";
                CarPct = "(-%)";
                FatPct = "(-%)";
            }
            else
            {
                CalPct = FormatPct(CurrentSummary.Actual.Calories, CurrentSummary.Target.Calories);
                ProPct = FormatPct(CurrentSummary.Actual.Protein, CurrentSummary.Target.Protein);
                CarPct = FormatPct(CurrentSummary.Actual.Carbs, CurrentSummary.Target.Carbs);
                FatPct = FormatPct(CurrentSummary.Actual.Fat, CurrentSummary.Target.Fat);
            }
        }
        */
        private string FormatPct(double? actual, double? target)
        {
            if (actual == null || target == null || target == 0)
            {
                return "(-%)";
            }
            return $"({ ((double)(( actual / target ) * 100)).ToString("0.0")}%)";
        }
    }
}
