using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.DataModels
{
    /// <summary>
    /// Holds a StartTime and EndTime DateTime to represent a period of time.
    /// </summary>
    /// <remarks> Should have had this earlier tbh. </remarks>
    public sealed class TimePeriod
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TimePeriod(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimePeriod(DateTime startTime, TimeSpan periodLength)
        {
            StartTime = startTime;
            EndTime = startTime + periodLength;
        }

        public TimePeriod(DateTime startTime, int days)
        {
            StartTime = startTime;
            EndTime = startTime.AddDays(days);
        }

        public TimePeriod StepForward(double step = 1)
        {
            TimeSpan span = EndTime - StartTime;
            return new TimePeriod(StartTime += span * step, EndTime += span * step);
        }

        public TimePeriod StepBack(double step = 1)
        {
            TimeSpan span = EndTime - StartTime;
            return new TimePeriod(StartTime -= span * step, EndTime -= span * step);
        }
    }

    public static class TimePeriodExtensions
    {
        public static TimeSpan GetTimeSpan(this TimePeriod timePeriod)
        {
            return timePeriod.EndTime - timePeriod.StartTime;
        }       

        /// <summary>
        /// Returns a string showing the time frame. 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="format"></param>
        /// <param name="inter"></param>
        /// <param name="pre"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public static string ToString(this TimePeriod period, string format, string inter = " - ", string pre = "", string post = "")
        {
            return $"{pre}{period.StartTime.ToString(format)}{inter}{period.EndTime.ToString(format)}{post}";
        }
    }
}
