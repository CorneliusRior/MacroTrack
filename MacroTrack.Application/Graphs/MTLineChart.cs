using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class MTLineChart : FrameworkElement
    {        
        /// <summary>
        /// Better debugging printer. Ignore all parameters except "Message", the rest fill in automatically.
        /// </summary>
        /// <param name="message"></param>
        public static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);

            // Define Chart area and draw Background:
            Rect chartArea = new Rect(0, 0, ActualWidth, ActualHeight);
            dc.DrawRectangle(BackgroundBrush, null, chartArea);

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

            // ClipToChartArea (put before axex & labels):
            if (ClipToChartArea) dc.PushClip(new RectangleGeometry(chartArea));
            else if (ClipToPlot) dc.PushClip(new RectangleGeometry(plot));


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
            

            // Originally was just contents of the else statements, but would crash when you opened an empty .db file.
            DateTime minX;
            DateTime maxX;
            double minY;
            double maxY; 
            if (allPoints.Count == 0) 
            {
                minX = XStart ?? DateTime.Today - XDefault;
                maxX = XEnd ?? DateTime.Today.AddDays(1);
                minY = YStart ?? 0;
                maxY = YEnd ?? minY + YDefault; 
            }
            else
            {
                minX = XStart ?? allPoints.Min(p => p.Time);
                maxX = XEnd ?? allPoints.Max(p => p.Time);
                minY = YStart ?? (ShowZero ? 0 : allPoints.Min(p => p.Value));
                maxY = YEnd ?? allPoints.Max(p => p.Value);
            }
            if (minX == maxX) maxX = maxX.AddDays(1); // This "Avoids divide by 0"
            if (minY == maxY) maxY += 1;

            // Draw Gridlines & Axis Labels:
            if (GridVerticalWeekly) DrawVerticalGridWeekly(dc, plot, dataPlot, minX, maxX);
            else DrawVerticalGridInterval(dc, plot, dataPlot, minX, maxX);
            DrawHorizontalGridLines(dc, plot, dataPlot, minY, maxY);

            // Apply clipping to non-data elements:
            if (ClipToChartArea) dc.Pop();

            // Define clipping for data elements:
            if (ClipToPlot) dc.PushClip(new RectangleGeometry(plot));
            else if (ClipToChartArea) dc.PushClip(new RectangleGeometry(chartArea));

            // Foreach DrawSeries
            foreach (var s in ss)
            {
                if (!s.Enabled) continue;
                if (s.SeriesType == SeriesType.LineContinuous) DrawLineSeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.LineDiscreteDaily) DrawLineDiscreteDailySeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.StepLine) DrawStepLineSeries(dc, dataPlot, s, minX, maxX, minY, maxY);
                if (s.SeriesType == SeriesType.TimePoints) DrawTimePointsSeries(dc, plot, dataPlot, s, minX, maxX);
                if (s.SeriesType == SeriesType.DaysBinary) DrawDaysBinarySeries(dc, plot, dataPlot, s, minX, maxX);
                if (s.SeriesType == SeriesType.Highlight) DrawHighlightSeries(dc, plot, dataPlot, s, minX, maxX);
            }

            // Apply clipping:
            if (ClipToPlot || ClipToChartArea) dc.Pop();
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
            HashSet<DateTime> exclusionHash = new();
            if (s.ExcludedPoints != null && s.ExcludedPoints.Count > 0) exclusionHash = GetExclusionHash(s.ExcludedPoints);
            if (s.ShowTrendline) DrawTrendLine(dc, dataPlot, s, minX, maxX, minY, maxY);
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null || pts.Count < 2) return;
            var pen = new Pen(Stroke, s.StrokeThickness);
            pen.DashStyle = s.SeriesDashStyle;
            if (pen.CanFreeze) pen.Freeze();
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(MapPoint(pts[0], dataPlot, minX, maxX, minY, maxY), false, false);
                for (int i = 1; i < pts.Count; i++) if (!exclusionHash.Contains(pts[i].Time.Date)) ctx.LineTo(MapPoint(pts[i], dataPlot, minX, maxX, minY, maxY), true, false); 
            }
            if (geo.CanFreeze) geo.Freeze();
            dc.DrawGeometry(null, pen, geo);
        }

        private void DrawLineDiscreteDailySeries(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            // FIx up data:
            DateTime startDate = minX.Date;
            DateTime endDate = maxX;
            var lookUp = s.DataPoints?.ToDictionary(p => p.Time.Date) ?? new Dictionary<DateTime, DataPoint>();
            List<DataPoint> pts = new();
            HashSet<DateTime> exclusionHash = new();
            if (s.ExcludedPoints != null && s.ExcludedPoints.Count > 0) exclusionHash = GetExclusionHash(s.ExcludedPoints);

            // Draw day before if present and that option is on, then define the data.
            if (DrawOneDayBefore) if (lookUp.TryGetValue(startDate.AddDays(-1), out var dp)) pts.Add(dp);
            for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
            {
                if (exclusionHash.Contains(i)) continue;
                if (lookUp.TryGetValue(i, out var dp)) pts.Add(dp);
                else pts.Add(new DataPoint { Time = i, Value = 0 });
            }
            
            if (s.ShowTrendline) DrawTrendLine(dc, dataPlot, s, minX, maxX, minY, maxY);

            // Draw the series (copy of DrawLineSeries really):
            Brush Stroke = GetSeriesStroke(s);
            var pen = new Pen(Stroke, s.StrokeThickness);
            pen.DashStyle = s.SeriesDashStyle;
            if (pen.CanFreeze) pen.Freeze();
            var geo = new StreamGeometry();
            TimeSpan? nudge = Settings.LineDiscreteOffsetToMid ? TimeSpan.FromHours(12) : null;
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(MapPoint(pts[0], dataPlot, minX, maxX, minY, maxY, nudge), false, false);
                for (int i = 1; i < pts.Count; i++) ctx.LineTo(MapPoint(pts[i], dataPlot, minX, maxX, minY, maxY, nudge), true, false);
            }
            if (geo.CanFreeze) geo.Freeze();
            dc.DrawGeometry(null, pen, geo);
        }

        private void DrawStepLineSeries(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            Brush Stroke = GetSeriesStroke(s);
            var pts = s.DataPoints;
            if (pts is null || pts.Count == 0) return; // We don't actually need 2, just one value really, can be just a horizontal line!
            Pen pen = new(Stroke, s.StrokeThickness);
            pen.DashStyle = s.SeriesDashStyle;
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
            pen.DashStyle = s.SeriesDashStyle;
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
            pen.DashStyle = s.SeriesDashStyle;
            if (pen.CanFreeze) pen.Freeze();
            Brush fillBrush = ApplyHatching(s);

            double y = plot.Top;
            double h = plot.Height - 1; // -1 do it doesn't draw over axis. Feel free to change it to -2
            double w = XWidthFromTimeSpan(TimeSpan.FromDays(1), dataPlot, minX, maxX);
            foreach (DataPoint p in pts)
            {
                if (p.Value != 0 && h > 0 && w > 0)
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
            pen.DashStyle = s.SeriesDashStyle;
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

        private void DrawTrendLine(DrawingContext dc, Rect dataPlot, PlotSeries s, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            var pts = s.DataPoints;
            if (pts == null || pts.Count < 2) return;
            int n = 0;
            double sumX = 0, sumY = 0, sumXX = 0, sumXY = 0;
            foreach (var p in pts)
            {
                // Trim ignore data from outside the bounds:
                if (p.Time < minX || p.Time > maxX || p.Value < minY || p.Value > maxY) continue;

                double x = (p.Time - minX).TotalDays;
                double y = p.Value;

                sumX += x;
                sumY += y;
                sumXX += x * x;
                sumXY += x * y;
                n++;
            }
            double denom = (n * sumXX) - (sumX * sumX);
            if (denom < 1e-12) return; // We'll deal with this later.
            double slope = ((n * sumXY) - (sumX * sumY)) / denom;
            double intercept = (sumY - (slope * sumX)) / n;

            // Get points for start and end:
            Point tl0 = MapPointFromElements(minX, intercept, dataPlot, minX, maxX, minY, maxY);
            Point tl1 = MapPointFromElements(maxX, intercept + (slope * (maxX - minX).TotalDays), dataPlot, minX, maxX, minY, maxY);
            var pen = new Pen(TrendLineBrush, Settings.LineTrendLineThickness);
            pen.DashStyle = DashStyles.Dash;
            if (pen.CanFreeze) pen.Freeze();
            dc.DrawLine(pen, tl0, tl1);

            if (s.ShowTrendLineStdDev)
            {
                if (pts.Count < 3) return;
                double sse = 0;
                foreach (DataPoint p in pts)
                {
                    double r = p.Value - ((slope * (p.Time - minX).TotalDays) + intercept);
                    sse += r * r;
                }
                double sigma = Math.Sqrt(sse / (n - 2));
                double delta = YWidthFromDouble(sigma, dataPlot, minY, maxY);
                var cpen = new Pen(TrendLineBrush, Settings.LineTrendLineThickness);
                cpen.DashStyle = DashStyles.Dot;
                if (cpen.CanFreeze) cpen.Freeze();
                Point cu0 = new(tl0.X, tl0.Y + delta);
                Point cu1 = new(tl1.X, tl1.Y + delta);
                Point cl0 = new(tl0.X, tl0.Y - delta);
                Point cl1 = new(tl1.X, tl1.Y - delta);
                dc.DrawLine(cpen, cu0, cu1);
                dc.DrawLine(cpen, cl0, cl1);
            }

            
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

        private static Point MapPoint(DataPoint p, Rect dataPlot, DateTime minX, DateTime maxX, double minY, double maxY, TimeSpan? nudgeX = null, double? nudgeY = null)
        {
            return new Point(MapX(p.Time, dataPlot, minX, maxX, nudgeX), MapY(p.Value, dataPlot, minY, maxY, nudgeY));
        }

        private static Point MapPointFromElements(DateTime x, double y, Rect dataPlot, DateTime minX, DateTime maxX, double minY, double maxY)
        {
            return new Point(MapX(x, dataPlot, minX, maxX), MapY(y, dataPlot, minY, maxY));
        }

        private static double MapX(DateTime x, Rect dataPlot, DateTime minX, DateTime maxX, TimeSpan? nudgeX = null)
        {
            double total = (maxX - minX).TotalSeconds;
            if (total <= 0) return dataPlot.Left;
            if (nudgeX is not null) x += nudgeX.Value;
            double t = (x - minX).TotalSeconds / total;
            return dataPlot.Left + t * dataPlot.Width;
        }

        private static double MapY(double y, Rect dataPlot, double minY, double maxY, double? nudgeY = null)
        {
            double total = (maxY - minY);
            if (total == 0) return dataPlot.Bottom;
            if (nudgeY is not null) y += nudgeY.Value;
            double t = (y - minY) / total;
            return dataPlot.Bottom - t * dataPlot.Height;
        }

        private static double XWidthFromTimeSpan(TimeSpan ts, Rect dataPlot, DateTime minX, DateTime maxX)
        {
            double total = (maxX - minX).TotalSeconds;
            if (total <= 0) return 0;
            return (ts.TotalSeconds / total) * dataPlot.Width; 
        }

        private static double YWidthFromDouble(double delta, Rect dataPlot, double minY, double maxY)
        {
            double total = maxY - minY;
            if (total == 0) return 0;
            return (delta / total) * dataPlot.Height;
        }

        private Brush GetSeriesStroke(PlotSeries s)
        {
            if (s.StrokeOverride is not null) return s.StrokeOverride;
            return (TryFindResource(s.SeriesColor.GetKey()) as Brush) ?? ErrorBrush;
        }

        private HashSet<DateTime> GetExclusionHash(IReadOnlyList<DataPoint> excludedPoints)
        {
            HashSet<DateTime> exclusionHash = new();
            foreach (DataPoint p in excludedPoints)
            {
                exclusionHash.Add(p.Time);
            }
            return exclusionHash;
            
        }
    }    
}
