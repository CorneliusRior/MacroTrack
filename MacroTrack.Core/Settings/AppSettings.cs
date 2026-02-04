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
        public bool LogInitMessages { get; set; } = true;
        public int LogRetainAmount { get; set; } = 20;
        public LogLevel LogUILevel { get; set; } = LogLevel.Warning;
        public LogLevel LogFileLevel { get; set; } = LogLevel.Debug;
        public int WeightMode { get; set; } = 0; // 0: kg, 1: lbs, 2: st
    
    
        public AppSettings Clone()
        {
            return new AppSettings
            {
                LogInitMessages = this.LogInitMessages,
                LogRetainAmount = this.LogRetainAmount,
                LogFileLevel = this.LogFileLevel,
                LogUILevel = this.LogUILevel,
                WeightMode = this.WeightMode
            };
        }

    }

    
}
