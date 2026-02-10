using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    public sealed class AppServices : IAppServices
    {
        public IAppEvents AppEvents { get; }
        // ...

        public AppServices()
        {
            AppEvents = new AppEvents();
            // ...
        }
    }
}
