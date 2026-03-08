using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    /// <summary>
    /// This thing is here such that if you set value of a textbox to null it goes blank.
    /// </summary>
    public class NullableDoubleConverter : IValueConverter
    {
        public object Convert(object? value, Type t, object? p, CultureInfo c)
        {
            return value is double d ? d.ToString(c) : "";
        }

        public object? ConvertBack(object? value, Type t, object p, CultureInfo c)
        {
            var s = (value as string)?.Trim();
            if (string.IsNullOrEmpty(s)) return null;
            return double.TryParse(s, NumberStyles.Float, c, out var d) ? d : null;
        }
    }
}
