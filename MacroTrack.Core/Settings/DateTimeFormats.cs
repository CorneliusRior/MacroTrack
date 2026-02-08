using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Settings
{
    public enum DTFormatS // "S" for "short"
    {
        Default,
        Default24Hour,
        dmy,
        dmy24Hour,
        myd,
        myd24Hour,
        Iso24,
        Iso12,
        ShortLocal
    }

    public sealed record DTFormatSItem(DTFormatS Value, string Format, string Display);

    public static class DTFormatSList
    {
        public static readonly IReadOnlyList<DTFormatSItem> List =
        [
            new(DTFormatS.Default,          "yyyy/M/d - h:mm tt",           "Default, 2026/2/7 - 7:42 PM"),
            new(DTFormatS.Default24Hour,    "yyyy/M/d - HH:mm",             "Default 24 Hour, 2026/2/7 - 19:42"),
            new(DTFormatS.dmy,              "d/M/yyyy - h:mm tt",           "Day/Month/Year, 7/2/2026 - 7:42 PM"),
            new(DTFormatS.dmy24Hour,        "d/M/yyyy - HH:MM",             "Day/Month/Year 24 Hour, 7/2/2026"),
            new(DTFormatS.myd,              "M/d/yyyy - h:mm tt",           "Month/Day/Year, 2/7/2026 - 7:42 PM"),
            new(DTFormatS.myd24Hour,        "M/d/yyyy - HH:mm",             "Month/Day/Year, 2/7/2026 - 19:42"),
            new(DTFormatS.Iso24,            "yyyy-MM-dd HH:mm",             "Iso24, 2026-02-07 07:42"),
            new(DTFormatS.Iso12,            "yyyy-MM-dd h:mm tt",           "Iso12, 2026-02-07 7:42 PM"),
            new(DTFormatS.ShortLocal,       "g",                            "Short local, locality based")
        ];

        public static readonly IReadOnlyDictionary<DTFormatS, string> FormatByValue = List.ToDictionary(i => i.Value, i => i.Format);
    }
        
    public enum DTFormatL // "L" for "long"
    {
        Default,
        Default24Hour,
        DefaultND, // "ND": "No days", as in days of the week.
        Default24HourND,
        myd,
        myd24Hour,
        mydND,
        myd24HourND,
        LongLocal
    }

    public sealed record DTFormatLItem(DTFormatL Value, string Format, string Display);

    public static class DTFormatLList
    {
        public static readonly IReadOnlyList<DTFormatLItem> List =
        [
            new(DTFormatL.Default,          "dddd, d MMMM, yyyy, h:mm:ss tt",      "Default, Friday, 7 February, 2026, 7:49 PM"),
            new(DTFormatL.Default24Hour,    "dddd, d MMMM, yyyy, HH:mm:ss",        "Default 24 Hour, Friday, 7 February, 2026, 19:49"),
            new(DTFormatL.DefaultND,        "d MMMM, yyyy, h:mm:ss tt",            "Default without weekdays, 7 February, 2026, 7:49 PM"),
            new(DTFormatL.Default24HourND,  "d MMMM, yyyy, HH:mm:ss",              "Default 24 Hour without weekdays, 7 February, 2026, 19:49"),
            new(DTFormatL.myd,              "dddd, MMMM d, yyyy, h:mm:ss tt",      "Month/Day/Year, Friday, February 7, 2026, 7:49 PM"),
            new(DTFormatL.myd24Hour,        "dddd, MMMM d, yyyy, HH:mm:ss",        "Month/Day/Year 24 Hour, Friday, February 7, 2026, 19:49"),
            new(DTFormatL.mydND,            "MMMM d, yyyy, h:mm:ss tt",            "Month/Day/Year without weekdays, February 7, 2026, 7:49 PM"),
            new(DTFormatL.myd24HourND,      "MMMM d, yyyy, HH:mm:ss",              "Month/Day/Year 24 Hour without weekdays, February 7, 2026, 19:49"),
            new(DTFormatL.LongLocal,        "G",                                   "Long local, locality based")
        ];

        public static readonly IReadOnlyDictionary<DTFormatL, string> FormatByValue = List.ToDictionary(i => i.Value, i => i.Format);
    }
}
