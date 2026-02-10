using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Services
{
    public interface IWindowService
    {
        void Show(WindowType type, object? parameter = null);
        bool? ShowDialog(WindowType type, object? parameter = null);
    }
}
