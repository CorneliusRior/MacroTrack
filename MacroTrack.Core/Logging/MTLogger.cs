using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace MacroTrack.Core.Logging
{
    public sealed class MTLogger : IMTLogger
    {
        public LogLevel UILevel { get; set; }
        public LogLevel FileLevel { get; set; }

        public event EventHandler<LogMessage>? MessageLogged;

        private readonly string _logFilePath;
        private readonly object _lock = new();

        public MTLogger(string logFilePath, string? program = "Unknown Program")
        {
            _logFilePath = logFilePath;
            Log(this, "Constructor", LogLevel.Info, $"New logger called by '{program ?? "Null"}'. UILevel = '{UILevel}', FileLevel = '{FileLevel}'");
        }

        public void Log(object sourceObj, string caller, LogLevel level, string message, Exception? ex = null)
        {
            string source = $"{sourceObj.GetType().FullName}.{caller}():";
            LogMessage msg = new LogMessage(
                Timestamp: DateTime.Now,
                Level: level,
                Message: message,
                Source: source ?? "",
                Exception: ex
                );

            if (level >= FileLevel)
            {
                var line = $"{msg.Timestamp:yyyy-MM-dd HH:mm:ss} [{msg.Level}] {msg.Source} {msg.Message}" + (ex is null ? "" : $" | {ex.GetType().Name}: {ex.Message}");
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, line + Environment.NewLine);
                }
            }

            if (level >= UILevel)
            {
                MessageLogged?.Invoke(this, msg);
            }
        }

        /// <summary>
        /// Logs name and value of variables supplied in an anonymous object
        /// Format like LogVars(new{ a, b, c } [...] )
        /// </summary>
        /// <example>
        /// <code>
        /// LogVars(new{ a, b, c }, "Variables before");
        /// </code>
        /// </example>
        /// <param name="sourceObj">Caller source</param>
        /// <param name="vars">An object whose public instances are logged</param>
        /// <param name="caller">Automatically supplied member name of caller, ignore.</param>
        /// <param name="prefix">String which proceeds the variable listing in the log entry</param>
        public void LogVars(object sourceObj, object vars, string caller, string? prefix = null)
        {
            var props = vars.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var parts = props.Select(p =>
            {
                var value = p.GetValue(vars);
                return $"{p.Name}={Format(value)}";
            });
            Log(sourceObj, caller, LogLevel.Debug, $"{(prefix ?? "LogVars:")} {string.Join(", ", parts)}");
        }

        private string Format(object? value)
        {
            if (value is null) return "null";
            return $"{value.GetType().Name} '{value}'";
        }

        public void OpenLogFile()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _logFilePath,
                UseShellExecute = true
            });
        }

        public void OpenLogDir()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(_logFilePath),
                UseShellExecute = true
            });
        }
    }
}
