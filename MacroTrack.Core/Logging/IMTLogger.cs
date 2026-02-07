using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Logging
{
    public interface IMTLogger
    {
        LogLevel UILevel { get; set; }
        LogLevel FileLevel { get; set; }

        event EventHandler<LogMessage>? MessageLogged;

        void Log(object sourceObj, string caller, LogLevel level, string message, Exception? exception = null);
        void LogVars(object sourceObj, object vars, string caller, string? prefix = null);
        void OpenLogFile();

        void OpenLogDir();
    }
}
