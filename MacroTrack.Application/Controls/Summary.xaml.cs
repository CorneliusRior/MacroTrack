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
        private readonly SummaryVM _vm = new();

        //Variables:
        public static readonly DependencyProperty CurrentSummaryProperty = DependencyProperty.Register(
            nameof(CurrentSummary),
            typeof(MacroSummary),
            typeof(Summary),
            new PropertyMetadata(null, (d, e) => ((Summary)d).Populate())
        );
        public MacroSummary? CurrentSummary
        {
            get => (MacroSummary?)GetValue(CurrentSummaryProperty);
            set => SetValue(CurrentSummaryProperty, value);
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

        public static readonly DependencyProperty CalPctProperty = DependencyProperty.Register(
            nameof(CalPct),
            typeof(string),
            typeof(Summary),
            new PropertyMetadata("(-%)")
        );
        public string CalPct
        {
            get => (string)GetValue(CalPctProperty); 
            set => SetValue(CalPctProperty, value);
        }

        public static readonly DependencyProperty ProPctProperty = DependencyProperty.Register(
            nameof(ProPct),
            typeof(string),
            typeof(Summary),
            new PropertyMetadata("(-%)")
        );
        public string ProPct
        {
            get => (string)GetValue(ProPctProperty);
            set => SetValue(ProPctProperty, value);
        }

        public static readonly DependencyProperty CarPctProperty = DependencyProperty.Register(
            nameof(CarPct),
            typeof(string),
            typeof(Summary),
            new PropertyMetadata("(-%)")
        );
        public string CarPct
        {
            get => (string)GetValue(CarPctProperty);
            set => SetValue(CarPctProperty, value);
        }

        public static readonly DependencyProperty FatPctProperty = DependencyProperty.Register(
            nameof(FatPct),
            typeof(string),
            typeof(Summary),
            new PropertyMetadata("(-%)")
        );
        public string FatPct
        {
            get => (string)GetValue(FatPctProperty);
            set => SetValue(FatPctProperty, value);
        }

        // Constructor & logic:

        public Summary()
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
            base.OnUnloaded(sender, e);
            _vm.OnClose();
        }


        public void Populate()
        {
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
