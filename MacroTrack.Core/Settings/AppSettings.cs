using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MacroTrack.Core.Logging;

namespace MacroTrack.Core.Settings
{
    public class AppSettings
    {
        // When adding a new setting, be sure to put it both here and in the Clone() method at the bottom.
        public string Theme { get; set; } = "Light";
        public string ThemeCustomForeground { get; set; } = "#FFFFFF00";
        public string GraphColorPro { get; set; } = "#FFFF0000";
        public string GraphColorCar { get; set; } = "#FF0000FF";
        public string GraphColorFat { get; set; } = "#FFFFFF00";
        public bool LogInitMessages { get; set; } = true;
        public int LogRetainAmount { get; set; } = 20;
        public LogLevel LogUILevel { get; set; } = LogLevel.Warning;
        public LogLevel LogFileLevel { get; set; } = LogLevel.Debug;
        public int WeightMode { get; set; } = 0; // 0: kg, 1: lbs, 2: st
        public bool ClockInLongFormat { get; set; } = true;
        public DTFormatS DTFormatShort { get; set; } = DTFormatS.Default; // Default, 2026/2/7 - 7:42 PM
        public DTFormatL DTFormatLong { get; set; } = DTFormatL.Default;


        public AppSettings Clone()
        {
            return new AppSettings
            {
                Theme = this.Theme,
                LogInitMessages = this.LogInitMessages,
                LogRetainAmount = this.LogRetainAmount,
                LogFileLevel = this.LogFileLevel,
                LogUILevel = this.LogUILevel,
                WeightMode = this.WeightMode,
                ClockInLongFormat = this.ClockInLongFormat,
                DTFormatShort = this.DTFormatShort,
                DTFormatLong = this.DTFormatLong
            };
        }

    }

    
}
