using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MacroTrack.AppLibrary.Validation
{
    public class NumericRequireRule : ValidationRule
    {
        public bool IsRequired { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!IsRequired) return ValidationResult.ValidResult;
            string? s = value as string;
            if (double.TryParse(s, out double n)) return ValidationResult.ValidResult;
            else return new ValidationResult(false, "Cannot parse");
        }
    }
}
