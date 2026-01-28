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
        public LogLevel LogUILevel { get; set; } = LogLevel.Warning;
        public LogLevel LogFileLevel { get; set; } = LogLevel.Debug;
        public int WeightMode { get; set; } = 0; // 0: kg, 1: lbs, 2: st
    }
}
