using MacroTrack.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Graphs
{
    /// <summary>
    /// Debug class which we can put in place of MacroBar to ensure that it is actually getting the data. It has the same data parameters, it just prints the data instead of drawing a graph.
    /// </summary>
    public class MacroBarDebug : FrameworkElement
    {
        public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(MacroSingleType),
            typeof(MacroBarDebug),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public MacroSingleType? Data
        {
            get => (MacroSingleType?)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var dpi = VisualTreeHelper.GetDpi(this);

            string text;
            /*
            if (Data is null)
            {
                text = "Data = null";
            }
            else
            {
                text = $"T={Data.Target}  A={Data.Actual}  Min={Data.TargetMin}  Max={Data.TargetMax}";
            }
            */

            var dcType = this.DataContext?.GetType().Name ?? "Null";
            var dataType = Data?.GetType().Name ?? "Null";
            text = $"DC={dcType}, Data={dataType}";

            var formatted = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                12,
                Brushes.Black,
                dpi.PixelsPerDip);

            dc.DrawText(formatted, new Point(4, 2));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(200, 20);
        }
    }
}
