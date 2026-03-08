using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Settings
{
    // Setting this as its own unified file and saying "Unit Formats" instead of just weight format because we might liek to use this in future, idk.
    public enum WeightFormat
    {
        Kg,
        Lbs,
        St
    }

    public static class WeightFormatExtensions
    {
        private const double KgToLb = 2.2046226218487757;
        private const double StToLb = 14;
        public static double ConvertTo(this WeightFormat inputFormat, WeightFormat outputFormat, double value)
        {            
            if (inputFormat == WeightFormat.Kg)
            {
                if (outputFormat == WeightFormat.Kg) return value;
                if (outputFormat == WeightFormat.Lbs) return value * KgToLb;
                if (outputFormat == WeightFormat.St) return (value * KgToLb) / StToLb;
            }
            if (inputFormat == WeightFormat.Lbs)
            {
                if (outputFormat == WeightFormat.Lbs) return value;
                if (outputFormat == WeightFormat.Kg) return value / KgToLb;
                if (outputFormat == WeightFormat.St) return value / StToLb;
            }
            if (inputFormat == WeightFormat.St)
            {
                if (outputFormat == WeightFormat.St) return value;
                if (outputFormat == WeightFormat.Kg) return (value * StToLb) / KgToLb;
                if (outputFormat == WeightFormat.Lbs) return (value * StToLb);
            }
            throw new ArgumentOutOfRangeException();
        }

        public static string ShortString(this WeightFormat fmt)
        {
            return fmt switch
            {
                WeightFormat.Kg => "Kg",
                WeightFormat.Lbs => "Lbs",
                WeightFormat.St => "St",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static string LongString(this WeightFormat fmt)
        {
            return fmt switch
            {
                WeightFormat.Kg => "Kilograms",
                WeightFormat.Lbs => "Pounds",
                WeightFormat.St => "Stone",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public sealed record WeightFormatListItem(WeightFormat Value, string ShortDisplay, string LongDisplay);

    public static class WeightFormatList
    {
        public static readonly IReadOnlyList<WeightFormatListItem> List =
        [
            new(WeightFormat.Kg, "Kg", "Kilograms"),
            new(WeightFormat.Lbs, "Lbs", "Pounds"),
            new(WeightFormat.St, "St", "Stone")
        ];
    }

}
