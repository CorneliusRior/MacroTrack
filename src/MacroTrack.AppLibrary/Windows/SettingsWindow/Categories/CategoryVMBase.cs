using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.AppLibrary.Windows.SettingsWindow.Categories
{
    public abstract class CategoryVMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
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

        // INotifyPropertyChanged stuff:
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Other methods:
        public void SetToDefault()
        {
            SettingsEditable = SettingsDefault;
        }

        public void SetToCurrent()
        {
            SettingsEditable = SettingsCurrent;
        }

        public static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }
    }
}
