using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    public sealed class AppServices : IAppServices
    {
        public CoreServices Services;
        public IAppEvents AppEvents { get; }
        public IWindowService WindowService { get; }
        // ...

        public AppServices(CoreServices services)
        {
            Services = services;
            AppEvents = new AppEvents();
            WindowService = new WindowService(Services, this);
            // ...
        }
    }
}
