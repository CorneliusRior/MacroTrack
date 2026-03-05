using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    public interface IAppEvents
    {
        IDisposable Subscribe<T>(Action<T> handler);
        void Publish<T>(T message);
    }
}
