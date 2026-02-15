using MacroTrack.Core.DataModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Graphs
{
    public class MacroBar : FrameworkElement
    {

        // Input variables:
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(MacroSingleType), typeof(MacroBar), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
        );
        public MacroSingleType? Data
        {
            get => ( MacroSingleType? )GetValue( DataProperty );
            set => SetValue( DataProperty, value );
        }

        // Styling
        public static readonly DependencyProperty BarBorderBrushProperty = DependencyProperty.Register(
            nameof(BarBorderBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Black)
        );
        public Brush BarBorderBrush
        {
            get => ( Brush )GetValue( BarBorderBrushProperty );
            set => SetValue ( BarBorderBrushProperty, value );
        }

        public static readonly DependencyProperty BarFillNeutralBrushProperty = DependencyProperty.Register(
            nameof(BarFillNeutralBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Yellow)
        );
        public Brush BarFillNeutralBrush
        {
            get => (Brush)GetValue(BarFillNeutralBrushProperty);
            set => SetValue(BarFillNeutralBrushProperty, value);
        }

        public static readonly DependencyProperty BackGroundBorderBrushProperty = DependencyProperty.Register(
            nameof(BackGroundBorderBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Gray)
        );
        public Brush BackGroundBorderBrush
        {
            get => (Brush)GetValue(BackGroundBorderBrushProperty);
            set => SetValue(BackGroundBorderBrushProperty, value);
        }

        public static readonly DependencyProperty BackGroundFillBrushProperty = DependencyProperty.Register(
            nameof(BackGroundFillBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.White)
        );
        public Brush BackGroundFillBrush
        {
            get => (Brush)GetValue(BackGroundFillBrushProperty);
            set => SetValue(BackGroundFillBrushProperty, value);
        }

        public static readonly DependencyProperty MinMaxMarkerBrushProperty = DependencyProperty.Register(
            nameof(MinMaxMarkerBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Red)
        );
        public Brush MinMaxMarkerBrush
        {
            get => (Brush)GetValue(MinMaxMarkerBrushProperty);
            set => SetValue(MinMaxMarkerBrushProperty, value);
        }

        // Turn these into Dependencyproperties later?:
        public double BarBorderThickness { get; set; } = 1.0;
        public double BackGroundBorderThickness { get; set; } = 1.0;
        public double MinMaxMarkerThickness { get; set; } = 2.0;
        public double OverflowSegmentWidth { get; set; } = 6.0;
        public double OverflowSegmentGap { get; set; } = 2.0;
        public double OverflowRatio { get; set; } = 0.3;
        public Thickness Padding { get; set; } = new Thickness(1);

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Data is null) return;

            // Pixel snapping
            var dpi = VisualTreeHelper.GetDpi(this);
            dc.PushGuidelineSet(new GuidelineSet());
            double w = Math.Max(0, ActualWidth);
            double h = Math.Max(0, ActualHeight);

            // defining area bar can live in:
            Rect inner = new Rect(
                Padding.Left,
                Padding.Top,
                Math.Max(0, w - Padding.Left - Padding.Right),
                Math.Max(0, h - Padding.Top - Padding.Bottom)
            );
            if (inner.Width <= 0 || inner.Height <= 0) return;

            // Define Target Box:
            double overFlowSpace = inner.Width * OverflowRatio;
            double targetBoxWidth = Math.Max(0, inner.Width - overFlowSpace);
            Rect targetBox = new Rect(inner.Left, inner.Top, targetBoxWidth, inner.Height);
            Pen borderPen = new Pen(BackGroundBorderBrush, BackGroundBorderThickness);
            dc.DrawRectangle(BackGroundFillBrush, borderPen, targetBox);

            // Calculations:
            Debug.WriteLine($"Data={Data}");
            Debug.WriteLine($"Data.Target={Data.Target}, Data.Actual={Data.Actual}, Data.Target={Data.Target}");
            double fillRatio = (Data.Target > 0) ? (Data.Actual / Data.Target) : 0;
            if (double.IsNaN(fillRatio) || double.IsInfinity(fillRatio)) fillRatio = 0;
            double insideRatio = Math.Max(0, Math.Min(fillRatio, 1));
            double fillWidth = targetBoxWidth * insideRatio;

            // Fill
            if (fillWidth > 0)
            {
                var fillRect = new Rect(targetBox.Left, targetBox.Top, fillWidth, targetBox.Height);
                Pen barPen = new Pen(BarBorderBrush, BarBorderThickness);
                dc.DrawRectangle(BarFillNeutralBrush, barPen, fillRect);
            }

            // Draw Min Marker:
            DrawMarker(dc, targetBox, Data.Target, Data.TargetMin);

            // OverFlow
            double overRatio = Math.Max(0, fillRatio - 1);
            if (OverflowRatio > 0)
            {
                // Draw max marker:
                DrawMarker(dc, targetBox, Data.Target, Data.TargetMax);
                double overWidth = overRatio * targetBox.Width;
                double x = targetBox.Right + (OverflowSegmentGap * 2);  // giving a bit more space, adjust if it's wrong!
                double y = targetBox.Top;
                double remainingSpace = Math.Min(overWidth, inner.Right - x);
                var segPen = new Pen(BarBorderBrush, BarBorderThickness);
                while (remainingSpace > 0)
                {
                    double segW = Math.Min(OverflowSegmentWidth, remainingSpace);
                    Rect segRect = new Rect(x, y, segW, targetBox.Height);
                    dc.DrawRectangle(BarBorderBrush, segPen, segRect);
                    x += OverflowSegmentWidth + OverflowSegmentGap;
                    remainingSpace -= OverflowSegmentGap + OverflowSegmentGap;
                }
            }
            dc.Pop();
        }

        private void DrawMarker(DrawingContext dc, Rect targetBox, double target, double? value)
        {
            if (target <= 0) return;
            if (value is null) return;
            double ratio = value.Value / target;
            double x = targetBox.Left + ratio * targetBox.Width;
            Pen minmaxPen = new Pen(MinMaxMarkerBrush, MinMaxMarkerThickness);
            dc.DrawLine(minmaxPen, new Point(x, targetBox.Top - 2), new Point(x, targetBox.Bottom + 2));
        }
    }
}
