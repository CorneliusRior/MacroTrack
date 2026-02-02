using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace MacroTrack.AppLibrary.Models
{
    /// <summary>
    /// For use in FoodEntry for Macros, or anywhere else useful.
    /// </summary>
    public class ScaledValue
    {
        public double? Base {  get; set; }

        public void SetFromDisplayed(double? displayed, double? multiplier)
        {
            if (displayed == null) 
            { 
                Base = null; 
                return; 
            }
            if (multiplier == 0) return; // base remains unchanged.
            if (multiplier == null) return;
            Base = displayed / multiplier;
        }

        public void SetBase(double? baseValue)
        {
            Base = baseValue; 
        }

        public double? GetDisplayed(double? multiplier)
        {
            double? v = (Base is null ? null : Base) * (multiplier is null ? 1 : multiplier);            
            return v is null ? null : (double)Math.Round(v.Value, 2);
        }
    }
}
