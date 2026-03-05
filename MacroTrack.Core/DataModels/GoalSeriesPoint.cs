using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.DataModels;

// GoalSeriesPoint is a single point in a list that we can put on a list which we can graph onto the daily calories time series. We will have it ordered by Id, not by Date, so that we can have two of these in the same day, so that there will be an instantaneous jump down upon goal changes.
public record GoalSeriesPoint
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Calories { get; set; }
}