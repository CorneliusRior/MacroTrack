using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Settings
{
    /// <summary>
    /// Settings for use in WPF graphs. Make sure to put any new Properties into the Clone() method below.
    /// </summary>
    public sealed class GraphSettings
    {
        public double LineTrendLineThickness { get; set; } = 1;
        public bool LineDiscreteOffsetToMid { get; set; } = true;

        public GraphSettings Clone()
        {
            return new GraphSettings
            {
                LineTrendLineThickness = this.LineTrendLineThickness,
                LineDiscreteOffsetToMid = this.LineDiscreteOffsetToMid
            };
        }
    }
}
