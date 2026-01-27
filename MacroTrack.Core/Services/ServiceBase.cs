using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Services
{
    public abstract class ServiceBase
    {
        protected IMTLogger Logger { get; }

        protected ServiceBase(CoreContext ctx) 
        {
            Logger = ctx.Logger;
            Log($"{this.GetType().Name} Initialised");
        }

        protected void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }
    }
}
