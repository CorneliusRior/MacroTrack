using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
/*
namespace MacroTrack.BasicApp.Forms
{
    public class BaseForm : Form
    {
        protected CoreServices? Services { get; private set; }
        protected IMTLogger? Logger => Services?.Logger;
#pragma warning disable CS0067 // event is never used
        public event EventHandler<string>? RequestPrint;
        public event EventHandler<string>? RequestPrintInline;
#pragma warning restore CS0067

        // Design constructor:
        public BaseForm() { }

        protected BaseForm(CoreServices services)
        {
            Services = services;
        }

        protected void InitServices(CoreServices services)
        {
            Services = services;
        }

        protected void Log(string message, LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger?.Log(this, caller, level, message, ex);
        }

        protected void Print(string text)
        {
            RequestPrint?.Invoke(this, text);
        }

        protected void PrintInline(string text)
        {
            RequestPrintInline?.Invoke(this, text);
        }
    }
}
*/