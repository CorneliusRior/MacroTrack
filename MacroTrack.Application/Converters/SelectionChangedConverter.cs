using MacroTrack.AppLibrary.ViewModels;
using MacroTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MacroTrack.AppLibrary.Converters
{
    class SelectionChangedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object p, CultureInfo c)
        {
            return new SelectionChanged((WeightEntry)values[0], values[1] as bool? == true);
        }

        public object[] ConvertBack(object value, Type[] t, object p, CultureInfo c)
        {
            throw new NotSupportedException();
        }
    }
}
