using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    public abstract class CategoryVMBase
    {
        public string Title { get; }
        public AppSettings SettingsDefault = new();
        public AppSettings SettingsEditable { get; set; }
        public AppSettings SettingsCurrent { get; }

        protected CategoryVMBase(string title, AppSettings settings)
        {
            Title = title;
            SettingsEditable = settings;
            SettingsCurrent = settings;
        }

        public void SetToDefault()
        {
            SettingsEditable = SettingsDefault;
        }

        public void SetToCurrent()
        {
            SettingsEditable = SettingsCurrent;
        }
    }
}
