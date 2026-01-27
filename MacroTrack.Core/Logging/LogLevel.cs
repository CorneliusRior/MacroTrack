using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Logging
{
    public enum LogLevel
    {
        // Levels of importance for printing and log messages. 0 only appears for debugging purposes, 1 for other feedback, 2 for more important warnings, 3 should be pushed to the end user and should not be disabled.

        // Almost every action should have a debug level message, method called and parameters. Info should be used when files are written, ideally only 1 or two per user action, used as a form of feedback. Warning should be displayed for errors which are for the most part handled, revserion to default, handling user error. Warnings should be reserved for actual fatal errors, database errors, crashes, things we need to start debugging for, and should present an Error message box.
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
