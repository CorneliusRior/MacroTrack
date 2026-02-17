using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Graphs
{
    /// <summary>
    /// A chart which can take data in the form of a set of Series. Series can be one of three types: Line, TimePoints, and DaysBinary. Give it data in the form of an IReadOnlyList<PlotSeries>.
    /// Line series is a regular line series. Time points generate a vertical line at that specific time, DaysBinary highlight an entire day.
    /// </summary>
    /// <remarks>
    /// Rewritten to just be a framework instead of a UserControl
    /// </remarks>
    public sealed class MTLineChart : FrameworkElement
    {
        // Changed functions:
        /*
        private static void OnSeriesSetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (MTLineChart)d;
            if (e.OldValue is INotifyCollectionChanged oldSS) oldSS.CollectionChanged -= chart.OnSeriesCollectionChanged;
            if (e.NewValue is INotifyCollectionChanged newSS) newSS.CollectionChanged += chart.OnSeriesCollectionChanged;
        }
        private void OnSeriesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
        }*/
        
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
            new FrameworkPropertyMetadata( new FontFamily("Segoe UI"), FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public FontFamily AxisLabelFont
        {
            get => (FontFamily)GetValue(AxisLabelFontProperty);
            set => SetValue(AxisLabelFontProperty, value);
        }

        // AxisLabelFontSize
        public static readonly DependencyProperty AxisLabelFontSizeProperty = DependencyProperty.Register(
            nameof(AxisLabelFontSize), typeof(double), typeof(MTLineChart),
            new FrameworkPropertyMetadata( 12.0, FrameworkPropertyMetadataOptions.AffectsRender)
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
            new FrameworkPropertyMetadata( "" , FrameworkPropertyMetadataOptions.AffectsRender)
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
            new FrameworkPropertyMetadata(new Thickness(5,5,5,5), FrameworkPropertyMetadataOptions.AffectsRender)
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

        // Could make this dependencyproperty tbh...
        private Brush ErrorBrush = Brushes.Magenta;

        // Function:
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            // Draw Background:
            dc.DrawRectangle(BackgroundBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // Define plot area (Apply PlotMargin):
            Rect plot = new Rect(
                PlotMargin.Left, PlotMargin.Top,
                Math.Max(0, ActualWidth - PlotMargin.Left - PlotMargin.Right),
                Math.Max(0, ActualHeight - PlotMargin.Top - PlotMargin.Bottom)
            );

            if (plot.Width <= 0 || plot.Height <= 0) return; // No space, can't do anything.

            Rect dataPlot = new Rect(
                plot.Left + DataMargin.Left, plot.Top + DataMargin.Top,
                Math.Max(0, plot.Width - DataMargin.Left - DataMargin.Right),
                Math.Max(0, plot.Height - DataMargin.Top - DataMargin.Bottom)
            );

            // Draw axes:
            var axisPen = new Pen(AxisBrush, 1);
            if (axisPen.CanFreeze) axisPen.Freeze();
            if (ShowXAxis) dc.DrawLine(axisPen, new Point(plot.Left, plot.Bottom), new Point(plot.Right, plot.Bottom));
            if (ShowYAxis) dc.DrawLine(axisPen, new Point(plot.Left, plot.Bottom), new Point(plot.Left, plot.Top));

            // Draw title if present:
            if (!string.IsNullOrWhiteSpace(Title))
            {
                double dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                Typeface titleTypeFace = new (TitleFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                FormattedText ft = new(Title, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, titleTypeFace, TitleFontSize, TitleBrush, dpi);
                dc.DrawText(ft, new Point( 
                    (ActualWidth / 2) - (ft.Width / 2) ,
                    (plot.Top - ft.Height - 5)
                ));
            }

            // Load SeriesSet, check that it's not null:
            var ss = SeriesSet;
            if (ss == null || ss.Count == 0)
            { 
                BlankPlot(dc, plot, dataPlot);
                DrawErrorMessage("Null Series Set", dc, plot);
                return; 
            } 
            
            // Determine min & max X & Y:
            var allPoints = ss.SelectMany(s => s.DataPoints ?? Array.Empty<DataPoint>()).ToList();
            DateTime minX = XStart ?? allPoints.Min(p => p.Time);
            DateTime maxX = XEnd ?? allPoints.Max(p => p.Time);
            if (minX == maxX) maxX = maxX.AddDays(1); // This "Avoids divide by 0"

            double minY = YStart ?? (ShowZero ? 0 : allPoints.Min(p => p.Value));
            double maxY = YEnd ?? allPoints.Max(p => p.Value);
            if (minY == maxY) maxY += 1;

            // Draw Gridlines & Axis Labels:
            if (GridVerticalWeekly) DrawVerticalGridWeekly(dc, plot, dataPlot, minX, maxX);
            else DrawVerticalGridInterval(dc, plot, dataPlot, minX, maxX);
            DrawHorizontalGridLines(dc, plot, dataPlot, minY, maxY);

            // foreach frawseries
            foreach (var s in ss)
            {                
                if (s.SeriesType == SeriesType.LineContinuous) DrawLineSeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.LineDiscreteDaily) DrawLineDiscreteDailySeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.StepLine) DrawStepLineSeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.TimePoints) DrawTimePointsSeries(dc, plot, dataPlot, s, minX, maxX);
                if (s.SeriesType == SeriesType.DaysBinary) DrawDaysBinarySeries(dc, plot, dataPlot, s, minX, maxX);
                if (s.SeriesType == SeriesType.Highlight) DrawHighlightSeries(dc, plot, dataPlot, s, minX, maxX);
            }
        }

        private void BlankPlot(DrawingContext dc, Rect plot, Rect dataPlot)
        {
            DateTime maxX = DateTime.Today.AddDays(1); // Tomorrow
            DateTime minX = maxX- XDefault;
            double minY = 0;
            double maxY = YDefault;
            if (GridVerticalWeekly) DrawVerticalGridWeekly(dc, plot, dataPlot, minX, maxX);
            else DrawVerticalGridInterval(dc, plot, dataPlot, minX, maxX);
            DrawHorizontalGridLines(dc, plot, dataPlot, minY, maxY);
        }

        private void DrawErrorMessage(string Message, DrawingContext dc, Rect plot)
        {
            // We'll use the title Font and Brush, because we don't want it to be too difficult to change everything.
            double dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            Typeface errorTypeFace = new (TitleFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            double fontSize = Math.Min(plot.Height * 0.2, plot.Width * 0.1);
            Brush brush = TitleBrush.Clone();
            brush.Opacity = ErrorMessageOpacity;
            FormattedText ft = new(Message, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, errorTypeFace, fontSize, brush, dpi);
            dc.DrawText(ft, new Point(
                PlotMargin.Left + (plot.Width / 2) - (ft.Width / 2),
                PlotMargin.Top + (plot.Height / 2) - (ft.Height / 2)
            ));
            
        }

        private void DrawVerticalGridWeekly(DrawingContext dc, Rect plot, Rect dataPlot, DateTime minX, DateTime maxX)
        {
            var pen = new Pen(GridBrush, 1);
            if (pen.CanFreeze) pen.Freeze();

            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var typeFace = new Typeface(AxisLabelFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            // Get first:
            int diff = ((int)minX.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime start = minX.AddDays(-diff);
            if (start < minX.Date) start = start.AddDays(7);
            int MaxLines = 100;
            int count = 0;
            for (DateTime x = start; x <= maxX; x = x.AddDays(7))
            {
                double px = MapX(x, dataPlot, minX, maxX);
                if (ShowGrid)
                {
                    dc.DrawLine(pen, new Point(px, plot.Top), new Point(px, plot.Bottom));
                    if (count++ >= MaxLines) break;
                }
                if (ShowXAxisLabels)
                {
                    string Glabel = TimeOnly.FromDateTime(x) == TimeOnly.MinValue ? x.ToString("HH:mm") : x.ToString("dd-YY");
                    string label = x.ToString("dd-MM");
                    var ft = new FormattedText(label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, AxisLabelFontSize, AxisBrush, dpi);
                    dc.DrawText(ft, new Point(px - ft.Width / 2, plot.Bottom + 2));
                }
            }
        }

        private void DrawVerticalGridInterval(DrawingContext dc, Rect plot, Rect dataPlot, DateTime minX, DateTime maxX)
        {
            var pen = new Pen(GridBrush, 1);
            if (pen.CanFreeze) pen.Freeze();

            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var typeFace = new Typeface(AxisLabelFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            TimeSpan range = maxX - minX;
            TimeSpan interval = GetTimeInterval(range, MaxHorizontalLines);
            // we will do this left to right so that we can have one line at "now":
            DateTime start = maxX;
            DateTime end = minX;
            int MaxLines = 100;
            int count = 0;
            for (DateTime x = start; x >= end; x -= interval)
            {
                double px = MapX(x, dataPlot, minX, maxX);
                if (ShowGrid)
                {                    
                    dc.DrawLine(pen, new Point(px, plot.Top), new Point(px, plot.Bottom));
                    if (count++ >= MaxLines) break;
                }
                if (ShowXAxisLabels)
                {
                    string label = x.ToString("dd-MM");
                    var ft = new FormattedText(label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, AxisLabelFontSize, AxisBrush, dpi);
                    dc.DrawText(ft, new Point(px - ft.Width / 2, plot.Bottom + 2));
                }
            }
        }

        private static TimeSpan GetTimeInterval(TimeSpan range, int maxHorizontalLines)
        {
            if (range <= TimeSpan.Zero) return TimeSpan.FromDays(1);
            foreach (TimeSpan t in TimeIntervalList)
            {
                if (range.TotalSeconds / t.TotalSeconds <= maxHorizontalLines) return t;
            }
            return TimeIntervalList[^1];
        }

        private static readonly TimeSpan[] TimeIntervalList =
        {
            TimeSpan.FromHours(1), TimeSpan.FromHours(3), TimeSpan.FromHours(6), TimeSpan.FromHours(12),
            TimeSpan.FromDays(1), TimeSpan.FromDays(7), TimeSpan.FromDays(14), TimeSpan.FromDays(30),
            TimeSpan.FromDays(60), TimeSpan.FromDays(90), TimeSpan.FromDays(180), TimeSpan.FromDays(365)
        };


        private void DrawHorizontalGridLines(DrawingContext dc, Rect plot, Rect dataPlot, double minY, double maxY)
        {
            var pen = new Pen(GridBrush, 1);
            if (pen.CanFreeze) pen.Freeze();

            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var typeFace = new Typeface(AxisLabelFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            double range = maxY - minY;
            double interval = GetInterval(range, MaxHorizontalLines);
            double start = Math.Floor(minY / interval) * interval;
            double end = Math.Ceiling(maxY / interval) * interval;
            int maxLines = 100; // to avoid infinite loops, will look odd, but still.
            int count = 0;
            for (double y = start; y <= end; y += interval)
            {
                double py = MapY(y, dataPlot, minY, maxY);
                if (py <= plot.Top || py >= plot.Bottom) continue;
                if (ShowGrid)
                {
                    dc.DrawLine(pen, new Point(plot.Left, py), new Point(plot.Right, py));
                    if (count++ > maxLines) break;
                }
                if (ShowYAxisLabels)
                {
                    string label = $"{YAxisPrefix}{y}{YAxisSuffix}";
                    var ft = new FormattedText(label, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, AxisLabelFontSize, AxisBrush, dpi);
                    dc.DrawText(ft, new Point(plot.Left - ft.Width - 5, py - ft.Height / 2));
                }
            }
        }

        private static readonly double[] IntervalList =
        {
            0.1, 0.25, 0.5,
            1, 2.5, 5,
            10, 25, 50,
            100, 250, 500,
            1000, 2500, 5000,
            10000, 25000, 50000
        };

        private static double GetInterval(double range, int maxVerticalLines)
        {
            if (range <= 0) return 1;
            foreach (double step in IntervalList)
            {
                if (range / step <= maxVerticalLines) return step;
            }
            return IntervalList[^1];
        }


        private void DrawLineSeries(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null || pts.Count < 2) return;
            var pen = new Pen(Stroke, s.StrokeThickness);
            if (pen.CanFreeze) pen.Freeze();
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(MapPoint(pts[0], dataPlot, minX, maxX, minY, maxY), false, false);
                for (int i = 1; i < pts.Count; i++) ctx.LineTo(MapPoint(pts[i], dataPlot, minX, maxX, minY, maxY), true, false);
            }
            if (geo.CanFreeze) geo.Freeze();
            dc.DrawGeometry(null, pen, geo);
        }
        
        private void DrawLineDiscreteDailySeries(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {

        }

        private void DrawStepLineSeries(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null || pts.Count == 0) return; // We don't actually need 2, just one value really, can be just a horizontal line!
            Pen pen = new(Stroke, s.StrokeThickness);
            if (pen.CanFreeze) pen.Freeze();
            StreamGeometry geo = new();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(MapPoint(pts[0], dataPlot, minX, maxX, minY, maxY), false, false);
                for (int i = 1; i < pts.Count; i++)
                {
                    // Draw Horizontal Line:
                    ctx.LineTo(new Point(MapX(pts[i].Time, dataPlot, minX, maxX),  MapY(pts[i-1].Value, dataPlot, minY, maxY)), true, false);
                    // Draw Vertical Line
                    ctx.LineTo(MapPoint(pts[i], dataPlot, minX, maxX, minY, maxY), true, false);
                }
                ctx.LineTo(new Point(MapX(maxX, dataPlot, minX, maxX), MapY(pts[^1].Value, dataPlot, minY, maxY)), true, false);
            }
            if (geo.CanFreeze) geo.Freeze();
            dc.DrawGeometry(null, pen, geo);
        }

        private void DrawTimePointsSeries(DrawingContext dc, Rect plot, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null) return;
            var pen = new Pen(Stroke, s.StrokeThickness);
            if (pen.CanFreeze) pen.Freeze();
            double y0 = plot.Bottom;
            double y1 = plot.Top;
            foreach (DataPoint p in pts)
            {
                if (p.Value != 0)
                {
                    double x = MapX(p.Time, dataPlot, minX, maxX);
                    dc.DrawLine(pen, new Point(x, y0), new Point(x, y1));
                }
            }
        }

        private void DrawDaysBinarySeries(DrawingContext dc, Rect plot, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null) return;
            var pen = new Pen(Stroke, s.StrokeThickness);
            if (pen.CanFreeze) pen.Freeze();
            Brush fillBrush = ApplyHatching(s);

            double y = plot.Top;
            double h = plot.Height;
            double w = XWidthFromTimeSpan(TimeSpan.FromDays(1), dataPlot, minX, maxX);
            foreach (DataPoint p in pts)
            {
                if (p.Value != 0)
                {
                    // Get the date of the thing:
                    DateTime dayDate = p.Time.Date;

                    dc.DrawRectangle(fillBrush, pen, new Rect(MapX(p.Time, dataPlot, minX, maxX), y, w, h));
                }
            }
        }

        private void DrawHighlightSeries(DrawingContext dc, Rect plot, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null || pts.Count < 2) return;
            var pen = new Pen(Stroke, s.StrokeThickness);
            if (pen.CanFreeze) pen.Freeze();
            Brush fillBrush = Stroke.Clone();
            fillBrush.Opacity = 0.4;

            // Determine start & end dates. We will just take the first 2 dates, anything after is junk data:
            DateTime startTime = pts[0].Time;
            DateTime endTime = pts[1].Time;
            double startPos = MapX(startTime, dataPlot, minX, maxX);
            double endPos = MapX(endTime, dataPlot, minX, maxX);
            double b = plot.Bottom;
            double y = plot.Top;
            double h = plot.Height;
            double w = XWidthFromTimeSpan(endTime - startTime, dataPlot, minX, maxX);

            // Draw rectangle first:
            dc.DrawRectangle(fillBrush, null, new Rect(startPos, y, w, h));

            //Then the vertical lines:
            dc.DrawLine(pen, new Point(startPos, b), new Point(startPos, y));
            dc.DrawLine(pen, new Point(endPos, b), new Point(endPos, y));
        }

        private Brush ApplyHatching(PlotSeries s)
        {
            Brush Stroke = GetSeriesStroke(s);
            Brush fillBrush = Stroke.Clone();
            fillBrush.Opacity = 0.4;
            if (!Hatch) return fillBrush;
            double tile = HatchSpacing * 2;
            Pen stripePen = new Pen(Stroke, s.StrokeThickness);
            if (stripePen.CanFreeze) stripePen.Freeze();
            GeometryGroup lines = new();
            for (double x = -tile; x <= tile * 2; x += HatchSpacing)
            {
                lines.Children.Add(new LineGeometry(new Point(x, tile * 2), new Point(x + tile * 2, 0)));
            }
            lines.Freeze();
            GeometryDrawing stripeDrawing = new(fillBrush, stripePen, lines);
            if (stripeDrawing.CanFreeze) stripeDrawing.Freeze();
            DrawingGroup dg = new();
            dg.Children.Add(stripeDrawing);
            if (dg.CanFreeze) dg.Freeze();
            DrawingBrush brush = new(dg)
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, tile, tile),
                ViewportUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.None
            };
            if (brush.CanFreeze) brush.Freeze();
            return brush;
        }

        private static Point MapPoint(DataPoint p, Rect dataPlot, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            return new Point(MapX(p.Time, dataPlot, minX, maxX), MapY(p.Value, dataPlot, minY, maxY));
        }

        private static double MapX(DateTime x, Rect dataPlot, DateTime minX, DateTime maxX)
        {
            double total = (maxX - minX).TotalSeconds;
            if (total <= 0) return dataPlot.Left;
            double t = (x - minX).TotalSeconds / total;
            return dataPlot.Left + t * dataPlot.Width;
        }

        private static double MapY(double y, Rect dataPlot, double minY, double maxY)
        {
            double total = (maxY - minY);
            if (total == 0) return dataPlot.Bottom;
            double t = (y - minY) / total;
            return dataPlot.Bottom - t * dataPlot.Height;
        }

        private Brush GetSeriesStroke(PlotSeries s)
        {
            if (s.StrokeOverride is not null) return s.StrokeOverride;
            return (TryFindResource(s.SeriesColor.GetKey()) as Brush) ?? ErrorBrush;
        }

        private static double XWidthFromTimeSpan(TimeSpan ts, Rect dataPlot, DateTime minX, DateTime maxX)
        {
            double total = (maxX - minX).TotalSeconds;
            if (total <= 0) return 0;
            return (ts.TotalSeconds / total) * dataPlot.Width; 
        }
    }


    /// <summary>
    /// Enum of types of Series we can use. Line is a classic line chart, TimePoints draws a vertical line at a particular time (continuous), DaysBinary draws a rectangle on single days (discrete) for which value = 1, or rather, value equal to anything other than 0.
    /// </summary>
    /// <remarks>
    /// Add the following:
    ///  - StepLine         (Changes in data are sudden, like for change in Goal or something)
    ///  - Highlight        (Highlights a period between two places, basically a continuous rectangle with two TimePoints on it)
    /// </remarks>
    public enum SeriesType
    { 
        LineContinuous,
        LineDiscreteDaily,
        StepLine,
        TimePoints,
        DaysBinary,
        Highlight
    }

    public readonly record struct DataPoint
    {
        public DateTime Time { get; init; }
        public double Value { get; init; }
    }

    /// <summary>
    /// SeriesColor denotes a particular color which can be altered with themes, formatted this way so as to maintain consistency. Could call it SeriesColorGroup, but we should avoid using very long names like that.
    /// </summary>
    public enum SeriesColor
    {
        Default,
        Error,

        LineSeriesBrush1,
        LineSeriesBrush2,
        LineSeriesBrush3,

        DayBinarySeriesBrush1,

        HighLight1
    }

    public static class SeriesColorExtensions
    {
        public static string GetKey(this SeriesColor c)
        {
            return c switch
            {
                SeriesColor.Default                 => "SeriesDefaultBrush",
                SeriesColor.Error                   => "SeriesErrorBrush",
                SeriesColor.LineSeriesBrush1        => "LineSeriesBrush1",
                SeriesColor.LineSeriesBrush2        => "LineSeriesBrush2",
                SeriesColor.LineSeriesBrush3        => "LineSeriesBrush3",
                SeriesColor.DayBinarySeriesBrush1   => "DayBinarySeriesBrush1",
                SeriesColor.HighLight1              => "HighlightBrush1", 
                _ => "SeriesErrorBrush" //"SeriesDefaultBrush" if you want to be more forgiving.
            };
        }
    }

    /// <summary>
    /// A class for giving data to MTChart. Contains parameters enum SeriesType, IReadOnlyList<DataPoint> DataPoints, Brush Stroke, and double StrokeThickness.
    /// </summary>
    public sealed class PlotSeries
    {
        public SeriesType SeriesType { get; init; }
        public IReadOnlyList<DataPoint>? DataPoints { get; set; }
        public SeriesColor SeriesColor { get; set; } = SeriesColor.Default;
        public Brush? StrokeOverride { get; init; }
        public double StrokeThickness { get; init; } = 2.0;        
    }
}
