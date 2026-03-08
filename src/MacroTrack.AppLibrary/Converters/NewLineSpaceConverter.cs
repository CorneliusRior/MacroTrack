using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    /// <summary>
    /// Makes multi-line strings 1 line, used in FoodLogEntryCards
    /// </summary>
    internal class NewLineSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
        {
            return value?.ToString()?.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ") ?? "";
        }
        public object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            return value;
        }
    }
}
