using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.DataModels;

public record CalSeriesPoint
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Calories { get; set; }
}
