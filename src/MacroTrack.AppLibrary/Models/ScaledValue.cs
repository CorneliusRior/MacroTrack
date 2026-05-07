using ClosedXML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public string DisplayStr { get; set; } = "";

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

        public void SetFromString(string newDisplayed, double? multiplier)
        {
            if (string.IsNullOrWhiteSpace(newDisplayed))
            {
                Base = null;
                DisplayStr = "";
                return;
            }
            if (multiplier == 0) return; // base remains unchanged.
            if (multiplier == null) return;
            double newVal = double.Parse(newDisplayed);
            Base = newVal / multiplier;

            DisplayStr = newDisplayed;
        }

        

        public void SetBase(double? baseValue)
        {
            Base = baseValue;
            DisplayStr = Base.ToString() ?? "";
        }

        public string GetDisplayed(double? multiplier, bool multUpdating)
        {
            if (multUpdating && Base != null)
            {
                DisplayStr = Math.Round(Base * multiplier ?? 1, 2).ToString();
            }
            return DisplayStr;
        }

        public double? GetValue(double? multiplier)
        {
            double? v = (Base is null ? null : Base) * (multiplier is null ? 1 : multiplier);            
            return v is null ? null : (double)Math.Round(v.Value, 2);
        }
    }    
}
