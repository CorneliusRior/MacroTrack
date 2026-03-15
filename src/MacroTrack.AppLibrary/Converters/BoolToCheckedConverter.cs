using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    public class BoolToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c) => value is bool b && b ? "✔" : "";
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => throw new NotSupportedException();
    }
}
