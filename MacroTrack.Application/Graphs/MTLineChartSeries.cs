using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Graphs
{
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
    /// SeriesColor denotes a particular color which can be altered with themes, formatted this way so as to maintain consistency. Could call it SeriesColorGroup, but we should avoid using very long names like that. Please note that this is AppSpecific, and will not work outside of MacroTrack unless themes/resources are set up in a very similar way.
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
                SeriesColor.Default => "SeriesDefaultBrush",
                SeriesColor.Error => "SeriesErrorBrush",
                SeriesColor.LineSeriesBrush1 => "LineSeriesBrush1",
                SeriesColor.LineSeriesBrush2 => "LineSeriesBrush2",
                SeriesColor.LineSeriesBrush3 => "LineSeriesBrush3",
                SeriesColor.DayBinarySeriesBrush1 => "DayBinarySeriesBrush1",
                SeriesColor.HighLight1 => "HighlightBrush1",
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
        public DashStyle SeriesDashStyle { get; set; } = DashStyles.Solid;
        public Brush? StrokeOverride { get; init; }
        public double StrokeThickness { get; init; } = 2.0;
        public bool ShowTrendline { get; init; } = false;
        public bool ShowTrendLineStdDev { get; init; } = false;
    }
}
