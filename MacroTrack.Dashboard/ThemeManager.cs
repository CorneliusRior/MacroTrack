using MacroTrack.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MacroTrack.Dashboard
{
    internal class ThemeManager
    {
        private const string ThemePrefix = "/MacroTrack.AppLibrary;component/Resources/Theme.";

        public static void SetLightDark(bool light)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains(ThemePrefix));
            if (existing != null) dictionaries.Remove(existing);
            var uri = new Uri(
                $"pack://application:,,,{ThemePrefix}{(light ? "Light" : "Dark")}.xaml",
                UriKind.Absolute);
            dictionaries.Insert(0, new ResourceDictionary { Source = uri });
        }

        public static void ToggleLightDark()
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source?.OriginalString.Contains(ThemePrefix) == true)?.Source?.OriginalString ?? "";
            SetLightDark(existing.Contains("Theme.Dark.xaml"));
        }
    }
}
