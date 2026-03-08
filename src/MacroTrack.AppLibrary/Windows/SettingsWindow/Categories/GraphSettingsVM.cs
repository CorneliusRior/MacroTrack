using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    internal class GraphSettingsVM : CategoryVMBase
    {
        public GraphSettingsVM(AppSettings settings) : base("Graph", settings)
        {
            p("GraphSettings called");
        }
    }
}
