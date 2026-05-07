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
            p($"SetFromDisplayed() called: Base = '{Base.ToString() ?? "null"}', displayed = '{displayed.ToString() ?? "null"}', multiplier = '{multiplier.ToString() ?? "null"}'");
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
            p($"SetFromString() called: Base = '{Base.ToString() ?? "null"}', DisplayStr = '{DisplayStr}', newDisplayed = '{newDisplayed}', multiplier = '{multiplier.ToString() ?? "null"}'");

            if (string.IsNullOrWhiteSpace(newDisplayed))
            {
                p("string.IsNullOrwhiteSpace(newDisplayed) == true;");
                Base = null;
                DisplayStr = "";
                return;
            }
            if (multiplier == 0) return; // base remains unchanged.
            if (multiplier == null) return;
            double newVal = double.Parse(newDisplayed);
            p($"newVal = {newVal}");
            Base = newVal / multiplier;

            DisplayStr = newDisplayed;
        }

        

        public void SetBase(double? baseValue)
        {
            p($"SetBase() called: Base = '{Base.ToString() ?? "null"}', baseValue = '{baseValue.ToString() ?? "null"}'");
            Base = baseValue;
            DisplayStr = Base.ToString() ?? "";
        }

        public string GetDisplayed(double? multiplier, bool multUpdating)
        {
            p($"GetDisplayed() called, multiplier = '{multiplier.ToString() ?? "null"}', multUpdating = '{multUpdating}' DisplayStr = '{DisplayStr}'");
            if (multUpdating && Base != null)
            {
                DisplayStr = Math.Round(Base * multiplier ?? 1, 2).ToString();
            }
            return DisplayStr;
        }

        public double? GetValue(double? multiplier)
        {
            p($"GetValue() called: Base = '{Base.ToString() ?? "null"}', multiplier = '{multiplier.ToString() ?? "null"}'");
            double? v = (Base is null ? null : Base) * (multiplier is null ? 1 : multiplier);            
            return v is null ? null : (double)Math.Round(v.Value, 2);
        }

        /// <summary>
        /// Better debugging printer. Ignore all parameters except "Message", the rest fill in automatically.
        /// </summary>
        /// <param name="message"></param>
        public static void p(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.WriteLine($"{Path.GetFileName(file)} line {line} {member}(): {message}");
        }
    }    
}
