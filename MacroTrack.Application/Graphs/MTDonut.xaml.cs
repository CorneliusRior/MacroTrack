using MacroTrack.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

namespace MacroTrack.AppLibrary.Graphs
{
    /// <summary>
    /// Interaction logic for MTDonut.xaml
    /// </summary>
    public partial class MTDonut : UserControl
    {
        public MTDonut()
        {
            InitializeComponent();
            SizeChanged += (_, _) => Redraw();
            Loaded += (_, _) => Redraw();
        }

        // Dependencies:

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register(
                nameof(Summary),
                typeof(MacroSummary),
                typeof(MTDonut),
                new PropertyMetadata(null, (d, e) => ((MTDonut)d).Redraw())
            );
        public MacroSummary? Summary
        {
            get => (MacroSummary?)GetValue(SummaryProperty);
            set => SetValue(SummaryProperty, value);
        }

        public static readonly DependencyProperty TotalTypeProperty = DependencyProperty.Register(
            nameof(TotalType),
            typeof(string),
            typeof(MTDonut),
            new PropertyMetadata("Target", (d, e) => ((MTDonut)d).Redraw())
        );
        public string TotalType
        {
            get => (String)GetValue(TotalTypeProperty);
            set => SetValue(TotalTypeProperty, value);
        }

        public static readonly DependencyProperty CenterTextProperty = DependencyProperty.Register(
                nameof(CenterText),
                typeof(string),
                typeof(MTDonut),
                new PropertyMetadata("Calories", (d, e) => ((MTDonut)d).UpdateCenterText())
            );
        public string CenterText
        {
            get => (string)GetValue(CenterTextProperty);
            set => SetValue(CenterTextProperty, value);
        }

        public static readonly DependencyProperty CenterValueProperty = DependencyProperty.Register(
                nameof(CenterValue),
                typeof(string),
                typeof(MTDonut),
                new PropertyMetadata("0", (d, e) => ((MTDonut)d).UpdateCenterText())
         );
        public string CenterValue
        {
            get => (string)GetValue(CenterValueProperty);
            set => SetValue(CenterValueProperty, value);
        }

        public static DependencyProperty LabelFontSizeProperty = DependencyProperty.Register(
            nameof(LabelFontSize),
            typeof(int),
            typeof(MTDonut),
            new PropertyMetadata(12, (d, e) => ((MTDonut)d).Redraw())
        );
        public int LabelFontSize
        {
            get => (int)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public static DependencyProperty ShowLabelProperty = DependencyProperty.Register(
            nameof(ShowLabels),
            typeof(bool),
            typeof(MTDonut),
            new PropertyMetadata(false, (d, e) => ((MTDonut)d).Redraw())
        );
        public bool ShowLabels
        {
            get => (bool)GetValue(ShowLabelProperty);
            set => SetValue(CenterValueProperty, value);
        }

        public static DependencyProperty HatchProperty = DependencyProperty.Register(
            nameof(Hatch),
            typeof(bool),
            typeof(MTDonut),
            new PropertyMetadata(true, (d, e) => ((MTDonut)d).Redraw())
        );
        public bool Hatch
        {
            get => (bool)GetValue(HatchProperty);
            set => SetValue(CenterValueProperty, value);
        }

        public static DependencyProperty HatchThicknessProperty = DependencyProperty.Register(
            nameof(HatchThickness),
            typeof(int),
            typeof(MTDonut),
            new PropertyMetadata(2, (d, e) => ((MTDonut)d).Redraw())
        );
        public int HatchThickness
        {
            get => (int)GetValue(HatchThicknessProperty);
            set => SetValue(HatchThicknessProperty, value);
        }

        public static DependencyProperty HatchSpacingProperty = DependencyProperty.Register(
            nameof(HatchSpacing),
            typeof(int),
            typeof(MTDonut),
            new PropertyMetadata(8, (d, e) => ((MTDonut)d).Redraw())
        );
        public int HatchSpacing
        {
            get => (int)GetValue(HatchSpacingProperty);
            set => SetValue(HatchSpacingProperty, value);
        }

        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register(
                nameof(BackgroundBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Black, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        public static readonly DependencyProperty LabelBrushProperty = DependencyProperty.Register(
                nameof(LabelBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Gold, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush LabelBrush
        {
            get => (Brush)GetValue(LabelBrushProperty);
            set => SetValue(LabelBrushProperty, value);
        }

        public static readonly DependencyProperty ProBrushProperty = DependencyProperty.Register(
                nameof(ProBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Red, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush ProBrush
        {
            get => (Brush)GetValue(ProBrushProperty);
            set => SetValue(ProBrushProperty, value);
        }

        public static readonly DependencyProperty CarBrushProperty = DependencyProperty.Register(
                nameof(CarBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Blue, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush CarBrush
        {
            get => (Brush)GetValue(CarBrushProperty);
            set => SetValue(CarBrushProperty, value);
        }

        public static readonly DependencyProperty FatBrushProperty = DependencyProperty.Register(
                nameof(FatBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Yellow, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush FatBrush
        {
            get => (Brush)GetValue(FatBrushProperty);
            set => SetValue(FatBrushProperty, value);
        }

        public static readonly DependencyProperty EmptyBrushProperty = DependencyProperty.Register(
                nameof(EmptyBrush),
                typeof(Brush),
                typeof(MTDonut),
                new PropertyMetadata(Brushes.Gray, (d, e) => ((MTDonut)d).Redraw())
        );
        public Brush EmptyBrush
        {
            get => (Brush)GetValue(EmptyBrushProperty);
            set => SetValue(EmptyBrushProperty, value);
        }

        public static readonly DependencyProperty InnerRadiusRatioProperty = DependencyProperty.Register(
            nameof(InnerRadiusRatio),
            typeof(double),
            typeof(MTDonut),
            new PropertyMetadata(0.6, (d, e) => ((MTDonut)d).Redraw())
        );
        public double InnerRadiusRatio
        {
            get => (double)GetValue(InnerRadiusRatioProperty);
            set => SetValue(InnerRadiusRatioProperty, value);
        }

        public static readonly DependencyProperty DonutPaddingProperty = DependencyProperty.Register(
            nameof(DonutPadding),
            typeof(int),
            typeof(MTDonut),
            new PropertyMetadata(30, (d, e) => ((MTDonut)d).Redraw())
        );
        public int DonutPadding
        {
            get => (int)GetValue(DonutPaddingProperty);
            set => SetValue(DonutPaddingProperty, value);
        }


        // Drawing:

        private void UpdateCenterText()
        {
            if (PART_CenterText != null) PART_CenterText.Text = CenterText;
            if (PART_CenterValue != null) PART_CenterValue.Text = CenterValue;
        }

        private void Redraw()
        {
            Debug.WriteLine("Redraw called");
            if (!IsLoaded || PART_Canvas == null) return;

            UpdateCenterText();
            PART_Canvas.Children.Clear();
            MacroTotals totals = new(0, 0, 0, 0);
            
            

            CenterValue = totals.Calories.ToString("0");

            double w = ActualWidth;
            double h = ActualHeight;
            if (w <= 1 || h <= 1) return;
            Debug.WriteLine($"ActualHeight={ActualHeight}, ActualWidth={ActualWidth}"); 

            double size = Math.Min(w, h);
            double outerR = (size / 2) - DonutPadding;
            double innerR = Math.Max(0, outerR * InnerRadiusRatio);

            var center = new Point(w / 2, h / 2);

            

            if (Summary != null)
            {
                if (Summary.NoGoal)
                {
                    GrayRing();
                    CenterText = "No goal";
                    CenterValue = "selected";
                    return;
                }
                if (TotalType == "Target") totals = Summary.Target;
                else if (TotalType == "Actual") totals = Summary.Actual;
                else if (TotalType == "Remaining") totals = Summary.Remaining;
                else throw new Exception($"Invalid totaltype '{TotalType}', must be 'Target', 'Actual', or 'Remaining'");
            }
            else
            {
                GrayRing();
                CenterText = "Null";
                CenterValue = "Summary";
                return;
            }

            double p = totals.Protein;
            double c = totals.Carbs;
            double f = totals.Fat;
            Debug.WriteLine($"p={p}, c={c}, f={f}");

            double pcf = p + c + f;

            if (pcf <= 0.000001)
            {
                CenterValue = "0";
                GrayRing();
                return;
            }

            void GrayRing()
            {
                var ring = CreateRingWedge(center, innerR, outerR, -90, 270);
                PART_Canvas.Children.Add(MakePath(ring, ApplyHatching(EmptyBrush), EmptyBrush));
                return;
            }

            // Draw actual thing: Start at top (90):
            double start = -90; 
            DrawSlice("Protein", p, pcf, ref start, center, innerR, outerR, ProBrush); 
            DrawSlice("Carbs", c, pcf, ref start, center, innerR, outerR, CarBrush); 
            DrawSlice("Fat", f, pcf, ref start, center, innerR, outerR, FatBrush);
            CenterValue = totals.Calories.ToString("0");

        }

        private void DrawSlice(string label, double value, double total, ref double startDeg, Point center, double innerR, double outerR, Brush color)
        {
            if (value <= 0) return;

            double sweep = ( value / total ) * 360;
            double endDeg = startDeg + sweep;

            var geom = CreateRingWedge(center, innerR, outerR, startDeg, endDeg);
            PART_Canvas.Children.Add(MakePath(geom, ApplyHatching(color), color));

            if (ShowLabels) DrawLabel(label, value, total, startDeg, endDeg, center, outerR);

            startDeg = endDeg;
        }

        private static Path MakePath(Geometry geom, Brush fill, Brush stroke)
        {
            return new Path
            {
                Data = geom,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = 2
            };
        }

        private Brush ApplyHatching(Brush baseBrush)
        {
            Debug.WriteLine("Made it to applyhatching");
            if (!Hatch) return baseBrush;
            if (baseBrush is not SolidColorBrush scb) return baseBrush;
            Debug.WriteLine("Not returning");
            //var c = scb.Color;

            double tile = HatchSpacing * 2;

            var stripePen = new Pen(scb, HatchThickness);
            stripePen.Freeze();

            var lines = new GeometryGroup();
            for (double x  = -tile; x <= tile * 2; x += HatchSpacing)
            {
                lines.Children.Add(new LineGeometry(new Point(x, tile * 2), new Point(x + tile * 2, 0)));
            }
            lines.Freeze();

            var stripeDrawing = new GeometryDrawing(null, stripePen, lines);
            stripeDrawing.Freeze();
            var dg = new DrawingGroup();
            dg.Children.Add(stripeDrawing);
            dg.Freeze();

            var brush = new DrawingBrush(dg)
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, tile, tile),
                ViewportUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.None
            };
            brush.Freeze();

            return brush;
        }

        private static Geometry CreateRingWedge(Point center, double innerR, double outerR, double startDeg, double endDeg)
        {
            double sweep = endDeg - startDeg;

            // draw one solid circle if it covers the whole thing.
            if (sweep >= 359)
            {
                var g = new GeometryGroup();
                g.Children.Add(new EllipseGeometry(center, outerR, outerR));
                g.Children.Add(new EllipseGeometry(center, innerR, innerR));
                return g;
            }

            bool largeArc = sweep > 180;

            // functions to make point creation easier:
            Point OuterPoint(double deg)
            {
                return PointOnCircle(center, outerR, deg);
            }
            Point InnerPoint(double deg)
            {
                return PointOnCircle(center, innerR, deg);
            }

            // Define 4 corners of segment:
            Point p1 = OuterPoint(startDeg);
            Point p2 = OuterPoint(endDeg);
            Point p3 = InnerPoint(endDeg);
            Point p4 = InnerPoint(startDeg);

            // Define figure
            PathFigure fig = new PathFigure { StartPoint = p1, IsClosed = true, IsFilled = true };

            // Draw outer arc:
            fig.Segments.Add(new ArcSegment
            {
                Point = p2,
                Size = new Size(outerR, outerR),
                IsLargeArc = largeArc,
                SweepDirection = SweepDirection.Clockwise
            });

            // Line from outer to inner arc:
            fig.Segments.Add(new LineSegment
            {
                Point = p3
            });

            // Arc back to P4:
            fig.Segments.Add(new ArcSegment
            {
                Point = p4,
                Size = new Size(innerR, innerR),
                IsLargeArc = largeArc,
                SweepDirection = SweepDirection.Counterclockwise
            });

            // Close arc:
            var geo = new PathGeometry();
            geo.Figures.Add(fig);
            geo.FillRule = FillRule.EvenOdd;
            return geo;
        }

        private static Point PointOnCircle(Point c, double r, double deg)
        {
            Debug.WriteLine($"PointonCircle Called, c={c}, r={r}, deg={deg}");
            double rad = deg * Math.PI / 180;
            return new Point(
                c.X + (r*Math.Cos(rad)),
                c.Y + (r*Math.Sin(rad))
            );
        }
    
        private void DrawLabel(string name, double value, double total, double startDeg, double EndDeg, Point center, double outerR)
        {
            double mid = (startDeg + EndDeg) / 2;
            double rad = mid * Math.PI / 180;

            double labelRadius = outerR + 50;
            double x = center.X + Math.Cos(rad) * labelRadius;
            double y = center.Y + Math.Sin(rad) * labelRadius;

            double percent = value / total;
            string text = $"{name}:\n{percent:P0}";

            var tb = new Label
            {
                Content = text,
                Foreground = LabelBrush,
                FontSize = LabelFontSize,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            PART_Canvas.Children.Add(tb);
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var size = tb.DesiredSize;
            Canvas.SetLeft(tb, x - size.Width / 2);
            Canvas.SetTop(tb, y - size.Height);
        }
    }
}
