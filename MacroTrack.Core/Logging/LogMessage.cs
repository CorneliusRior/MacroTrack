using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Logging
{
    public record LogMessage(
        DateTime Timestamp,
        LogLevel Level,
        string Message,
        string Source,
        Exception? Exception = null
    );
}
