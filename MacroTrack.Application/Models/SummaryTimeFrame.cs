using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Models
{
    public enum SummaryTimeFrame
    {
        CalendarDay,
        Last24Hours,
        CalendarWeek,
        Last7Days,
        CalendarMonth,
        Last30Days
    }

    public static class SummaryTimeFrameValues
    {
        public static readonly IReadOnlyList<TimeFrameItem> List =
        [
            new(SummaryTimeFrame.CalendarDay, "Calendar Day"),
            new(SummaryTimeFrame.Last24Hours, "Last 24 Hours"),
            new(SummaryTimeFrame.CalendarWeek, "Calendar Week"),
            new(SummaryTimeFrame.Last7Days, "Last 7 Days"),
            new(SummaryTimeFrame.CalendarMonth, "Calendar Month"),
            new(SummaryTimeFrame.Last30Days, "Last 30 days")
        ];
    }
    

    public sealed record TimeFrameItem(SummaryTimeFrame Value, string Display);


}
