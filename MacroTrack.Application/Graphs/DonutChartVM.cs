using MacroTrack.Core.Settings;
using OxyPlot;
using OxyPlot.Series;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Graphs
{
    public sealed class DonutChartVM
    {
        public PlotModel PlotModel { get; }
        public AppSettings Settings { get; }

        public DonutChartVM(AppSettings settings)
        {
            PlotModel = new PlotModel { Title = "PlotTitle" };
            Settings = settings;

            PlotModel.Background = OxyColor.Parse("#FF000000");

            var pie = new PieSeries
            {
                InnerDiameter = 0.6,
                StrokeThickness = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            pie.Slices.Add(new PieSlice("Protein", 40) { Fill = OxyColor.Parse(settings.GraphColorPro) });
            pie.Slices.Add(new PieSlice("Carbs", 40) { Fill = OxyColor.Parse(settings.GraphColorCar) });
            pie.Slices.Add(new PieSlice("Fat", 40) { Fill = OxyColor.Parse(settings.GraphColorFat) });

            PlotModel.Series.Add(pie);
        }
    }
}
