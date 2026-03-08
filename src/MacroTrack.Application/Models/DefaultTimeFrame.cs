using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Models
{
    /// <summary>
    /// Set of timeframes, not unlike that of SummaryTimeFrame but no calendar snapping.
    /// For use in places where you have a list of things, right now we're using this for DiaryView
    /// </summary>
    public enum DefaultTimeFrame
    {
        Day,
        Week,
        Month,
        Year,
        All,
        Custom
    }

    public static class DefaultTimeFrameExtensions
    {
        public static DateTime GetStartDate(this DefaultTimeFrame timeFrame, bool asDate = true, bool includeToday = true)
        {
            DateTime EndDate;
            if (asDate) EndDate = includeToday ? DateTime.Today.AddDays(1) : DateTime.Today;
            else EndDate = DateTime.Now;
            return timeFrame switch
            {
                DefaultTimeFrame.Day => EndDate.AddDays(-2), // 2 to include yesterday.
                DefaultTimeFrame.Week => EndDate.AddDays(-7),
                DefaultTimeFrame.Month => EndDate.AddMonths(-1),
                DefaultTimeFrame.Year => EndDate.AddYears(-1),
                DefaultTimeFrame.All => DateTime.MinValue,
                DefaultTimeFrame.Custom => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException(),
            };
        }

        public static DateTime GetEndDate(this DefaultTimeFrame timeFrame, bool asDate = true, bool includeToday = true)
        {
            DateTime startDate;
            if (asDate) startDate = includeToday ? DateTime.Today : DateTime.Today.AddDays(1);
            else startDate = DateTime.Now;
            return timeFrame switch
            {
                DefaultTimeFrame.Day => startDate.AddDays(1),
                DefaultTimeFrame.Week => startDate.AddDays(7),
                DefaultTimeFrame.Month => startDate.AddMonths(1),
                DefaultTimeFrame.Year => startDate.AddYears(1),
                DefaultTimeFrame.All => DateTime.MaxValue,
                DefaultTimeFrame.Custom => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
        }
    }
;


    public static class DefaultTimeFrameValues
    {
        public static readonly IReadOnlyList<TimeFrameItem> List =
        [
            new(DefaultTimeFrame.Day,       "Last Day"),
            new(DefaultTimeFrame.Week,      "Last Week"),
            new(DefaultTimeFrame.Month,     "Last Month"),
            new(DefaultTimeFrame.Year,      "Last Year"),
            new(DefaultTimeFrame.All,       "Show all"),
            new(DefaultTimeFrame.Custom,    "Custom")
        ];

        
    }

    public sealed record TimeFrameItem(DefaultTimeFrame Value, string Display);
}
