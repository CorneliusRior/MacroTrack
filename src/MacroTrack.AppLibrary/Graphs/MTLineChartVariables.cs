using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Graphs
{
    public partial class MTLineChart
    {
        // Settings
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            nameof(Settings), typeof(GraphSettings), typeof(MTLineChart),
            new FrameworkPropertyMetadata(new GraphSettings(), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public GraphSettings Settings
        {
            get => (GraphSettings)GetValue(SettingsProperty);
            set
            {
                SetValue(SettingsProperty, value);
            }
        }

        // Data
        public static readonly DependencyProperty SeriesSetProperty = DependencyProperty.Register(
            nameof(SeriesSet), typeof(IReadOnlyList<PlotSeries>), typeof(MTLineChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public IReadOnlyList<PlotSeries>? SeriesSet
        {
            get => (IReadOnlyList<PlotSeries>?)GetValue(SeriesSetProperty);
            set => SetValue(SeriesSetProperty, value);
        }

        // XDefault (timespan, we subtract this from DateTime.Now basically.
        public static readonly DependencyProperty XDefaultProperty = DependencyProperty.Register(
            nameof(XDefault), typeof(TimeSpan), typeof(MTLineChart),
            new FrameworkPropertyMetadata(TimeSpan.FromDays(30), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public TimeSpan XDefault
        {
            get => (TimeSpan)GetValue(XDefaultProperty);
            set => SetValue(XDefaultProperty, value);
        }

        // XStart:
        public static readonly DependencyProperty XStartProperty = DependencyProperty.Register(
            nameof(XStart), typeof(DateTime?), typeof(MTLineChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public DateTime? XStart
        {
            get => (DateTime?)GetValue(XStartProperty);
            set => SetValue(XStartProperty, value);
        }

        // XEnd:
        public static readonly DependencyProperty XEndProperty = DependencyProperty.Register(
            nameof(XEnd), typeof(DateTime?), typeof(MTLineChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public DateTime? XEnd
        {
            get => (DateTime?)GetValue(XEndProperty);
            set => SetValue(XEndProperty, value);
        }

        // YDefault
        public static readonly DependencyProperty YDefaultProperty = DependencyProperty.Register(
            nameof(YDefault), typeof(double), typeof(MTLineChart),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double YDefault
        {
            get => (double)GetValue(YDefaultProperty);
            set => SetValue(YDefaultProperty, value);
        }

        // YStart
        public static readonly DependencyProperty YStartProperty = DependencyProperty.Register(
            nameof(YStart), typeof(double?), typeof(MTLineChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double? YStart
        {
            get => (double?)GetValue(YStartProperty);
            set => SetValue(YStartProperty, value);
        }

        // YEnd
        public static readonly DependencyProperty YEndProperty = DependencyProperty.Register(
            nameof(YEnd), typeof(double?), typeof(MTLineChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double? YEnd
        {
            get => (double?)GetValue(YEndProperty);
            set => SetValue(YEndProperty, value);
        }

        // Styling DPs:

        // Options:
        // ShowGrid
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register(
            nameof(ShowGrid), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowGrid
        {
            get => (bool)GetValue(ShowGridProperty);
            set => SetValue(ShowGridProperty, value);
        }

        // ClipToChartArea
        public static readonly DependencyProperty ClipToChartAreaProperty = DependencyProperty.Register(
            nameof(ClipToChartArea), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ClipToChartArea
        {
            get => (bool)GetValue(ClipToChartAreaProperty);
            set => SetValue(ClipToChartAreaProperty, value);
        }

        // ClipToPlot
        public static readonly DependencyProperty ClipToPlotProperty = DependencyProperty.Register(
            nameof(ClipToPlot), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ClipToPlot
        {
            get => (bool)GetValue(ClipToPlotProperty);
            set => SetValue(ClipToPlotProperty, value);
        }

        // DrawOneDayBefore (In LineDiscreteDaily, have it check for the value of the day before for continuous looking graphs.)
        public static readonly DependencyProperty DrawOneDayBeforeProperty = DependencyProperty.Register(
            nameof(DrawOneDayBefore), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool DrawOneDayBefore
        {
            get => (bool)GetValue(DrawOneDayBeforeProperty);
            set => SetValue(DrawOneDayBeforeProperty, value);
        }

        // ShowXAxis
        public static readonly DependencyProperty ShowXAxisProperty = DependencyProperty.Register(
            nameof(ShowXAxis), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowXAxis
        {
            get => (bool)GetValue(ShowXAxisProperty);
            set => SetValue(ShowXAxisProperty, value);
        }

        // ShowYAxis
        public static readonly DependencyProperty ShowYAxisProperty = DependencyProperty.Register(
            nameof(ShowYAxis), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowYAxis
        {
            get => (bool)GetValue(ShowYAxisProperty);
            set => SetValue(ShowYAxisProperty, value);
        }

        // ShowXAxisLabels
        public static readonly DependencyProperty ShowXAxisLabelsProperty = DependencyProperty.Register(
            nameof(ShowXAxisLabels), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowXAxisLabels
        {
            get => (bool)GetValue(ShowXAxisLabelsProperty);
            set => SetValue(ShowXAxisLabelsProperty, value);
        }

        // ShowYAxisLabels
        public static readonly DependencyProperty ShowYAxisLabelsProperty = DependencyProperty.Register(
            nameof(ShowYAxisLabels), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowYAxisLabels
        {
            get => (bool)GetValue(ShowYAxisLabelsProperty);
            set => SetValue(ShowYAxisLabelsProperty, value);
        }

        // AxisLabelFont
        public static readonly DependencyProperty AxisLabelFontProperty = DependencyProperty.Register(
            nameof(AxisLabelFont), typeof(FontFamily), typeof(MTLineChart),
            new FrameworkPropertyMetadata(new FontFamily("Segoe UI"), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public FontFamily AxisLabelFont
        {
            get => (FontFamily)GetValue(AxisLabelFontProperty);
            set => SetValue(AxisLabelFontProperty, value);
        }

        // AxisLabelFontSize
        public static readonly DependencyProperty AxisLabelFontSizeProperty = DependencyProperty.Register(
            nameof(AxisLabelFontSize), typeof(double), typeof(MTLineChart),
            new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double AxisLabelFontSize
        {
            get => (double)GetValue(AxisLabelFontSizeProperty);
            set => SetValue(AxisLabelFontSizeProperty, value);
        }

        // Hatch (This only applies to DayBinary really)
        public static readonly DependencyProperty HatchProperty = DependencyProperty.Register(
            nameof(Hatch), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool Hatch
        {
            get => (bool)GetValue(HatchProperty);
            set => SetValue(HatchProperty, value);
        }

        // HatchSpacing
        public static readonly DependencyProperty HatchSpacingProperty = DependencyProperty.Register(
            nameof(HatchSpacing), typeof(int), typeof(MTLineChart),
            new FrameworkPropertyMetadata(8, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public int HatchSpacing
        {
            get => (int)GetValue(HatchSpacingProperty);
            set => SetValue(HatchSpacingProperty, value);
        }

        // MaxVerticalLines:
        public static readonly DependencyProperty MaxVerticalLinesProperty = DependencyProperty.Register(
            nameof(MaxVerticalLines), typeof(int), typeof(MTLineChart),
            new FrameworkPropertyMetadata(6, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public int MaxVerticalLines
        {
            get => (int)GetValue(MaxVerticalLinesProperty);
            set => SetValue(MaxVerticalLinesProperty, value);
        }

        // MaxHorizontalLines:
        public static readonly DependencyProperty MaxHorizontalLinesProperty = DependencyProperty.Register(
            nameof(MaxHorizontalLines), typeof(int), typeof(MTLineChart),
            new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public int MaxHorizontalLines
        {
            get => (int)GetValue(MaxHorizontalLinesProperty);
            set => SetValue(MaxHorizontalLinesProperty, value);
        }

        // Title (No bool for this one, we just set it as an empty string otherwise)
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(MTLineChart),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // TitleFont
        public static readonly DependencyProperty TitleFontProperty = DependencyProperty.Register(
            nameof(TitleFont), typeof(FontFamily), typeof(MTLineChart),
            new FrameworkPropertyMetadata(new FontFamily("Segoe UI"), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public FontFamily TitleFont
        {
            get => (FontFamily)GetValue(TitleFontProperty);
            set => SetValue(TitleFontProperty, value);
        }

        // TitleFontSize
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            nameof(TitleFontSize), typeof(double), typeof(MTLineChart),
            new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        // ErrorMessageOpacity
        public static readonly DependencyProperty ErrorMessageOpacityProperty = DependencyProperty.Register(
            nameof(ErrorMessageOpacity), typeof(double), typeof(MTLineChart),
            new FrameworkPropertyMetadata(0.6, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public double ErrorMessageOpacity
        {
            get => (double)GetValue(ErrorMessageOpacityProperty);
            set => SetValue(ErrorMessageOpacityProperty, value);
        }

        // YAxis Prefix
        public static readonly DependencyProperty YAxisPrefixProperty = DependencyProperty.Register(
            nameof(YAxisPrefix), typeof(string), typeof(MTLineChart),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public string YAxisPrefix
        {
            get => (string)GetValue(YAxisPrefixProperty);
            set => SetValue(YAxisPrefixProperty, value);
        }

        // YAxis Suffix
        public static readonly DependencyProperty YAxisSuffixProperty = DependencyProperty.Register(
            nameof(YAxisSuffix), typeof(string), typeof(MTLineChart),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public string YAxisSuffix
        {
            get => (string)GetValue(YAxisSuffixProperty);
            set => SetValue(YAxisSuffixProperty, value);
        }

        // ShowZero
        public static readonly DependencyProperty ShowZeroProperty = DependencyProperty.Register(
            nameof(ShowZero), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool ShowZero
        {
            get => (bool)GetValue(ShowZeroProperty);
            set => SetValue(ShowZeroProperty, value);
        }

        // Weekly Vertical
        public static readonly DependencyProperty GridVerticalWeeklyProperty = DependencyProperty.Register(
            nameof(GridVerticalWeekly), typeof(bool), typeof(MTLineChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public bool GridVerticalWeekly
        {
            get => (bool)GetValue(GridVerticalWeeklyProperty);
            set => SetValue(GridVerticalWeeklyProperty, value);
        }

        // PlotMargin:
        public static readonly DependencyProperty PlotMarginProperty = DependencyProperty.Register(
            nameof(PlotMargin), typeof(Thickness), typeof(MTLineChart),
            new FrameworkPropertyMetadata(new Thickness(10, 10, 10, 10), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Thickness PlotMargin
        {
            get => (Thickness)GetValue(PlotMarginProperty);
            set => SetValue(PlotMarginProperty, value);
        }

        // DataMargin: (margin around the mins & maxs basically, so lines don't hit the very edge)
        public static readonly DependencyProperty DataMarginProperty = DependencyProperty.Register(
            nameof(DataMargin), typeof(Thickness), typeof(MTLineChart),
            new FrameworkPropertyMetadata(new Thickness(5, 5, 5, 5), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Thickness DataMargin
        {
            get => (Thickness)GetValue(DataMarginProperty);
            set => SetValue(DataMarginProperty, value);
        }


        // Brushes:
        //Background Brush
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register(
            nameof(BackgroundBrush), typeof(Brush), typeof(MTLineChart),
            new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        // TitleBrush
        public static readonly DependencyProperty TitleBrushProperty = DependencyProperty.Register(
            nameof(TitleBrush), typeof(Brush), typeof(MTLineChart),
            new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Brush TitleBrush
        {
            get => (Brush)GetValue(TitleBrushProperty);
            set => SetValue(TitleBrushProperty, value);
        }

        // GridBrush
        public static readonly DependencyProperty GridBrushProperty = DependencyProperty.Register(
            nameof(GridBrush), typeof(Brush), typeof(MTLineChart),
            new FrameworkPropertyMetadata(Brushes.Gray,
                FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Brush GridBrush
        {
            get => (Brush)GetValue(GridBrushProperty);
            set => SetValue(GridBrushProperty, value);
        }

        // AxisBrush
        public static readonly DependencyProperty AxisBrushProperty = DependencyProperty.Register(
            nameof(AxisBrush), typeof(Brush), typeof(MTLineChart),
            new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Brush AxisBrush
        {
            get => (Brush)GetValue(AxisBrushProperty);
            set => SetValue(AxisBrushProperty, value);
        }

        // TrendLineBrush
        public static readonly DependencyProperty TrendLineBrushProperty = DependencyProperty.Register(
            nameof(TrendLineBrush), typeof(Brush), typeof(MTLineChart),
            new FrameworkPropertyMetadata(Brushes.Gray,
                FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public Brush TrendLineBrush
        {
            get => (Brush)GetValue(TrendLineBrushProperty);
            set => SetValue(TrendLineBrushProperty, value);
        }

        // Could make this dependencyproperty tbh...
        private Brush ErrorBrush = Brushes.Magenta;
        
    }
}
