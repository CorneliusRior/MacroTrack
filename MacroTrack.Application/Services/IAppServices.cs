using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    public interface IAppServices
    {
        public IAppEvents AppEvents { get; }
        //IWindowsService Windows..
    }
}
