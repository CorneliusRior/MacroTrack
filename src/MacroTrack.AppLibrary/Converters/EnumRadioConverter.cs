using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    /// <summary>
    /// Used to make radiobuttons w/ enums
    /// </summary>
    public sealed class EnumRadioConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
        {
            return value.Equals(p) == true;
        }

        public object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            return (value is bool b && b) ? p! : Binding.DoNothing;
        }
    }
}
