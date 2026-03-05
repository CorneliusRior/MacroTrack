using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    class EnumDateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object p, CultureInfo c)
        {
            if (values.Length < 2) return "";
            if (values[0] is not DateTime dt) return "";
            if (values[1] is not string fmt || string.IsNullOrWhiteSpace(fmt)) return dt.ToString(c);
            try { return dt.ToString(fmt, c); }
            catch (FormatException) { return dt.ToString(c); }
        }

        public object[] ConvertBack(object value, Type[] t, object p, CultureInfo c)
        {
            throw new NotSupportedException();
        }
    }
}
