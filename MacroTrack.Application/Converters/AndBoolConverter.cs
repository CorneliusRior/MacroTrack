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
    /// Enables multibinding for multiple bools
    /// </summary>
    public class AndBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object p, CultureInfo c)
        {
            foreach (var v in values)
            {
                if (v is bool b && !b) return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] t, object p, CultureInfo c) => throw new NotImplementedException();
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c) => value is bool b ? !b : value;
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }
}
