using MacroTrack.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MacroTrack.AppLibrary.Resources
{
    public static class ThemeManager
    {
        private const string ThemePrefix = "/MacroTrack.AppLibrary;component/Resources/Theme.";

        private static readonly Dictionary<string, Uri> Themes = new()
        {
            ["Light"] = new Uri($"pack://application:,,,{ThemePrefix}Light.xaml",
                UriKind.Absolute),
            ["Dark"] = new Uri($"pack://application:,,,{ThemePrefix}Dark.xaml",
                UriKind.Absolute),
            ["Dark custom"] = new Uri($"pack://application:,,,{ThemePrefix}DarkCustom.xaml",
                UriKind.Absolute),
            ["Wire"] = new Uri($"pack://application:,,,{ThemePrefix}Wire.xaml",
                UriKind.Absolute)
        };

        public static IReadOnlyList<string> GetThemeList()
        {
            return Themes.Keys.ToList();
        }

        public static void SetTheme(string themeName)
        {
            if (!Themes.TryGetValue(themeName, out var uri))
            {
                throw new ArgumentException($"Unknown theme '{themeName}'", nameof(themeName));
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains(ThemePrefix));
            if (existing != null) dictionaries.Remove(existing);
            dictionaries.Insert(0, new ResourceDictionary { Source = uri });
        }

        public static string GetCurrentTheme()
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existing = dictionaries.FirstOrDefault(d => d.Source?.OriginalString.Contains(ThemePrefix) == true)?.Source?.OriginalString ?? "";
            if (existing != null) return existing;
            else return "Light";
        }

        public static void SetCustomColors(AppSettings settings)
        {
            Application.Current.Resources["ThemeCustomForeground"] = ConvertToColor(settings.ThemeCustomForeground);
        }

        private static Color ConvertToColor(string s)
        {
            return (Color)ColorConverter.ConvertFromString(s)!;
        }
    }
}
