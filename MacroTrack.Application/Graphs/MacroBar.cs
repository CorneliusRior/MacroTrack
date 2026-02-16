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
            get => (MacroSingleType?)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        // Styling
        public static readonly DependencyProperty BarBorderBrushProperty = DependencyProperty.Register(
            nameof(BarBorderBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Black)
        );
        public Brush BarBorderBrush
        {
            get => (Brush)GetValue(BarBorderBrushProperty);
            set => SetValue(BarBorderBrushProperty, value);
        }

        public static readonly DependencyProperty BarFillGoodBrushProperty = DependencyProperty.Register(
            nameof(BarFillGoodBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Green)
        );
        public Brush BarFillGoodBrush
        {
            get => (Brush)GetValue(BarFillGoodBrushProperty);
            set => SetValue(BarFillGoodBrushProperty, value);
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

        public static readonly DependencyProperty BarFillBadBrushProperty = DependencyProperty.Register(
            nameof(BarFillBadBrush), typeof(Brush), typeof(MacroBar),
            new PropertyMetadata(Brushes.Red)
        );
        public Brush BarFillBadBrush
        {
            get => (Brush)GetValue(BarFillBadBrushProperty);
            set => SetValue(BarFillBadBrushProperty, value);
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

        // Other parametersL
        public static readonly DependencyProperty BarBorderThicknessProperty = DependencyProperty.Register(
            nameof(BarBorderThickness), typeof(double), typeof(MacroBar),
            new PropertyMetadata(1.0)
        );
        public Double BarBorderThickness // We keep this as a double because it is used in a pen, same with other "thicknesses" here defined as doubles.
        {
            get => (double)GetValue(BarBorderThicknessProperty);
            set => SetValue(BarBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty BackGroundBorderThicknessProperty = DependencyProperty.Register(
            nameof(BackGroundBorderThickness), typeof(double), typeof(MacroBar),
            new PropertyMetadata(1.0)
        );
        public double BackGroundBorderThickness
        {
            get => (double)GetValue(BackGroundBorderThicknessProperty);
            set => SetValue(BackGroundBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty MinMaxMarkerThicknessProperty = DependencyProperty.Register(
            nameof(MinMaxMarkerThickness), typeof(double), typeof(MacroBar),
            new PropertyMetadata(2.0)
        );
        public double MinMaxMarkerThickness
        {
            get => (double)GetValue(MinMaxMarkerThicknessProperty);
            set => SetValue(MinMaxMarkerThicknessProperty, value);
        }

        public static readonly DependencyProperty OverflowSegmentWidthProperty = DependencyProperty.Register(
            nameof(OverflowSegmentWidth), typeof(double), typeof(MacroBar),
            new PropertyMetadata(6.0)
        );
        public double OverflowSegmentWidth
        {
            get => (double)GetValue(OverflowSegmentWidthProperty);
            set => SetValue(OverflowSegmentWidthProperty, value);
        }

        public static readonly DependencyProperty OverflowSegmentGapProperty = DependencyProperty.Register(
            nameof(OverflowSegmentGap), typeof(double), typeof(MacroBar),
            new PropertyMetadata(2.0)
        );
        public double OverflowSegmentGap
        {
            get => (double)GetValue(OverflowSegmentGapProperty);
            set => SetValue(OverflowSegmentGapProperty, value);
        }

        public static readonly DependencyProperty OverflowRatioProperty = DependencyProperty.Register(
            nameof(OverflowRatio), typeof(double), typeof(MacroBar),
            new PropertyMetadata(0.3)
        );
        public double OverflowRatio
        {
            get => (double)GetValue(OverflowRatioProperty);
            set => SetValue(OverflowRatioProperty, value);
        }

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(PaddingProperty), typeof(Thickness), typeof(MacroBar),
            new PropertyMetadata(new Thickness(1.0))
        );
        public Thickness InnerPadding // We think of padding as internal.
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

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
            Rect outer = new Rect(0, 0, w, h);
            Rect inner = new Rect(
                InnerPadding.Left,
                InnerPadding.Top,
                Math.Max(0, w - InnerPadding.Left - InnerPadding.Right),
                Math.Max(0, h - InnerPadding.Top - InnerPadding.Bottom)
            );
            if (inner.Width <= 0 || inner.Height <= 0) return;

            // Define Target Box:
            double overFlowSpace = inner.Width * OverflowRatio;
            double fillBoxWidth = Math.Max(0, inner.Width - overFlowSpace);
            Rect targetBox = new Rect(outer.Left, outer.Top, fillBoxWidth + InnerPadding.Left + InnerPadding.Right, h);
            Rect fillBox = new Rect(inner.Left, inner.Top, fillBoxWidth, inner.Height);
            Pen borderPen = new Pen(BackGroundBorderBrush, BackGroundBorderThickness);
            dc.DrawRectangle(BackGroundFillBrush, borderPen, targetBox);

            // Calculations:
            double fillRatio = (Data.Target > 0) ? (Data.Actual / Data.Target) : 0;
            if (double.IsNaN(fillRatio) || double.IsInfinity(fillRatio)) fillRatio = 0;
            double insideRatio = Math.Max(0, Math.Min(fillRatio, 1));
            double fillWidth = fillBoxWidth * insideRatio;

            // Determine fill brush.
            double thresholdRatio = 0.1; // we will have this set in settings, but for now, we'll jus do 10%.
            Brush BarFillBrush = GetFillBrush(thresholdRatio);
            

            Brush GetFillBrush(double t) // we will have this altered depending on cut/bulk/maintenance eventually, maybe with a class "BarColorRules", for now, we'll just hardcode this in:
            {
                if (Data.TargetMin == null && Data.TargetMax == null)
                {
                    if (Data.Actual < Data.Target * (1 - t)) return BarFillNeutralBrush;
                    if (Data.Actual > Data.Target * (1 + t)) return BarFillBadBrush;
                    return BarFillGoodBrush;
                }
                if (Data.TargetMin != null && Data.TargetMax == null)
                {
                    if (Data.Actual < Data.TargetMin) return BarFillBadBrush;
                    if (Data.Actual > Data.Target * (1 + t)) return BarFillBadBrush;
                    if (Data.Actual < Data.Target * (1 - t)) return BarFillNeutralBrush;
                    return BarFillGoodBrush;
                }
                if (Data.TargetMin == null && Data.TargetMax != null)
                {
                    if (Data.Actual < Data.Target * (1 - t)) return BarFillBadBrush;
                    if (Data.Actual > Data.TargetMax) return BarFillBadBrush;
                    if (Data.Actual > Data.Target * (1 + t)) return BarFillNeutralBrush;
                    return BarFillGoodBrush;
                }
                if (Data.Actual < Data.TargetMin) return BarFillBadBrush;
                if (Data.Actual > Data.TargetMax) return BarFillBadBrush;
                if (Data.Actual < Data.Target * (1 - t)) return BarFillNeutralBrush;
                if (Data.Actual > Data.Target * (1 + t)) return BarFillNeutralBrush;
                return BarFillGoodBrush;
            }

            // Fill
            if (fillWidth > 0)
            {
                var fillRect = new Rect(fillBox.Left, fillBox.Top, fillWidth, fillBox.Height);
                Pen barPen = new Pen(BarBorderBrush, BarBorderThickness);
                dc.DrawRectangle(BarFillBrush, barPen, fillRect);
            }

            // Draw Min Marker:
            DrawMarker(dc, fillBox, Data.Target, Data.TargetMin);

            // OverFlow
            double overRatio = Math.Max(0, fillRatio - 1);
            if (overRatio > 0)
            {
                // Draw max marker:
                DrawMarker(dc, fillBox, Data.Target, Data.TargetMax);
                double overWidth = overRatio * fillBox.Width;
                double x = fillBox.Right + (OverflowSegmentGap * 2);  // giving a bit more space, adjust if it's wrong!
                double y = fillBox.Top;
                double remainingSpace = Math.Min(overWidth, inner.Right - x);
                var segPen = new Pen(BarBorderBrush, BarBorderThickness);
                while (remainingSpace > 0)
                {
                    double segW = Math.Min(OverflowSegmentWidth, remainingSpace);
                    Rect segRect = new Rect(x, y, segW, fillBox.Height);
                    dc.DrawRectangle(BarFillBrush, segPen, segRect);
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
            dc.DrawLine(minmaxPen, new Point(x, targetBox.Top), new Point(x, targetBox.Bottom));
        }
    }
}
