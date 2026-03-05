using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    public static class DTFormatSExtensions
    {
        public static string GetFormatString(this DTFormatS format, bool includeSeconds = false)
        {
            if (includeSeconds)
            {
                return format switch
                {
                    DTFormatS.Default       => "yyyy/M/d - h:mm:ss tt",
                    DTFormatS.Default24Hour => "yyyy/M/d - HH:mm:ss",
                    DTFormatS.dmy           => "d/M/yyyy - h:mm:ss tt",
                    DTFormatS.dmy24Hour     => "d/M/yyyy - HH:mm:ss",
                    DTFormatS.myd           => "M/d/yyyy - h:mm:ss tt",
                    DTFormatS.myd24Hour     => "M/d/yyyy - HH:mm:ss",
                    DTFormatS.Iso24         => "yyyy-MM-dd HH:mm:ss",
                    DTFormatS.Iso12         => "yyyy-MM-dd h:mm:ss tt",
                    DTFormatS.ShortLocal    => "g",
                    _ => "yyyy/M/d - h:mm:ss tt" // Revert to default
                };
            }
            return format switch
            {
                DTFormatS.Default       => "yyyy/M/d - h:mm tt",
                DTFormatS.Default24Hour => "yyyy/M/d - HH:mm",
                DTFormatS.dmy           => "d/M/yyyy - h:mm tt",
                DTFormatS.dmy24Hour     => "d/M/yyyy - HH:MM",
                DTFormatS.myd           => "M/d/yyyy - h:mm tt",
                DTFormatS.myd24Hour     => "M/d/yyyy - HH:mm",
                DTFormatS.Iso24         => "yyyy-MM-dd HH:mm",
                DTFormatS.Iso12         => "yyyy-MM-dd h:mm tt",
                DTFormatS.ShortLocal    => "g",
                _ => "yyyy/M/d - h:mm tt" // Revert to default
            };
        }
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

    public enum DFormatS
    {
        ymd,
        ymdShortYear,
        ymdFullDigits,
        ymdSYFD,        //short year full digits
        dmy,
        dmyShortYear,
        dmyFullDigits,
        dmySYFD,      
        mdy,
        mdyShortYear,
        mdyFullDigits,
        mdySYFD      
    }

    public static class DFormatSExtensions
    {
        public static string GetFormatString(this DFormatS format, string DateSeperator = "/")
        {
            string s = DateSeperator;
            return format switch
            {
                DFormatS.ymd            => $"yyyy{s}M{s}d",
                DFormatS.ymdShortYear   => $"yy{s}M{s}d",
                DFormatS.ymdFullDigits  => $"yyyy{s}MM{s}dd",
                DFormatS.ymdSYFD        => $"yy{s}MM{s}DD",
                DFormatS.dmy            => $"d{s}M{s}yyyy",
                DFormatS.dmyShortYear   => $"d{s}M{s}dd",
                DFormatS.dmyFullDigits  => $"dd{s}MM{s}yyyy",
                DFormatS.dmySYFD        => $"dd{s}MM{s}yy",
                DFormatS.mdy            => $"m{s}d{s}yyyy",
                DFormatS.mdyShortYear   => $"m{s}d{s}yy",
                DFormatS.mdyFullDigits  => $"mm{s}dd{s}yyyy",
                DFormatS.mdySYFD        => $"mm{s}dd{s}yy",
                _ => $"yyyy{s}M{s}d" // Revert to default.
            };
        }
    }

    public sealed record DFormatSListItem(DFormatS Value, string Display);

    public static class DFormatSList
    {
        public static readonly IReadOnlyList<DFormatSListItem> List =
        [
            new(DFormatS.ymd           ,  "2026/2/7"),
            new(DFormatS.ymdShortYear  ,  "26/2/7"),
            new(DFormatS.ymdFullDigits ,  "2026/02/07"),
            new(DFormatS.ymdSYFD       ,  "26/02/07"),
            new(DFormatS.dmy           ,  "7/2/2026"),
            new(DFormatS.dmyShortYear  ,  "7/2/26"),
            new(DFormatS.dmyFullDigits ,  "07/02/2026"),
            new(DFormatS.dmySYFD       ,  "07/02/26"),
            new(DFormatS.mdy           ,  "2/7/2026"),
            new(DFormatS.mdyShortYear  ,  "2/7/26"),
            new(DFormatS.mdyFullDigits ,  "02/07/2026"),
            new(DFormatS.mdySYFD       ,  "02/07/26"),
        ];
    }

    public enum DFormatL
    {
        dayDateMonthYear,
        dateMonthYear,
        dayMonthDateYear,
        MonthDateYear,
    }

    public static class DFormatLExtensions
    {
        public static string GetFormatString(this DFormatL format)
        {
            return format switch
            {
                DFormatL.dayDateMonthYear   => "dddd, d MMMM, yyyy",
                DFormatL.dateMonthYear      => "d MMMM yyyy",
                DFormatL.dayMonthDateYear   => "dddd, MMMM d, yyyy",
                DFormatL.MonthDateYear      => "MMMM d yyyy",
                _ => "dddd, d MMMM, yyyy" // Revert to default
            };
        }
    }

    public sealed record DFormatLListItem(DFormatL Value, string Display);

    public static class DFormatLList
    {
        public static readonly IReadOnlyList<DFormatLListItem> List =
        [
            new(DFormatL.dayDateMonthYear, "Friday, 7 February, 2026"),
            new(DFormatL.dateMonthYear,    "7 February 2026"),
            new(DFormatL.dayMonthDateYear, "Friday, February 7, 2026"),
            new(DFormatL.MonthDateYear,    "February 7 2026")
        ];
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

    public static class DTFormatLExtensions
    {
        public static string GetFormatString(this DTFormatL format, bool includeSeconds = false)
        {
            if (includeSeconds)
            {
                return format switch
                {
                    DTFormatL.Default           => "dddd, d MMMM, yyyy, h:mm:ss tt",
                    DTFormatL.Default24Hour     => "dddd, d MMMM, yyyy, HH:mm:ss",  
                    DTFormatL.DefaultND         => "d MMMM, yyyy, h:mm:ss tt",      
                    DTFormatL.Default24HourND   => "d MMMM, yyyy, HH:mm:ss",        
                    DTFormatL.myd               => "dddd, MMMM d, yyyy, h:mm:ss tt",
                    DTFormatL.myd24Hour         => "dddd, MMMM d, yyyy, HH:mm:ss",  
                    DTFormatL.mydND             => "MMMM d, yyyy, h:mm:ss tt",      
                    DTFormatL.myd24HourND       => "MMMM d, yyyy, HH:mm:ss",        
                    DTFormatL.LongLocal         => "G",
                    _ => "dddd, d MMMM, yyyy, h:mm:ss tt" // revert to default
                };
            }
            return format switch
            {
                DTFormatL.Default               => "dddd, d MMMM, yyyy, h:mm tt",
                DTFormatL.Default24Hour         => "dddd, d MMMM, yyyy, HH:mm",
                DTFormatL.DefaultND             => "d MMMM, yyyy, h:mm tt",
                DTFormatL.Default24HourND       => "d MMMM, yyyy, HH:mm",
                DTFormatL.myd                   => "dddd, MMMM d, yyyy, h:mm tt",
                DTFormatL.myd24Hour             => "dddd, MMMM d, yyyy, HH:mm",
                DTFormatL.mydND                 => "MMMM d, yyyy, h:mm tt",
                DTFormatL.myd24HourND           => "MMMM d, yyyy, HH:mm",
                DTFormatL.LongLocal             => "G",
                _ => "dddd, d MMMM, yyyy, h:mm tt" // revert to default
            };
        }
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

    public enum TimeFormat
    {
        Default,
        TwentyFourHour,
    }

    public static class TimeFormatExtentions
    {
        public static string GetFormatString(this TimeFormat format, bool includeSeconds = true, string timeSeperator = ":")
        {
            string t = timeSeperator;
            if (includeSeconds)
            {
                return format switch
                {
                    TimeFormat.Default => $"h{t}mm{t}ss tt",
                    TimeFormat.TwentyFourHour => $"HH{t}mm{t}ss",
                    _ => $"h{t}mm{t}ss tt" // revert to default
                };
            }
            return format switch
            {
                TimeFormat.Default => $"h{t}mm tt",
                TimeFormat.TwentyFourHour => $"HH{t}mm",
                _ => $"h{t}mm tt" // revert to default
            };
        }
    }

    public sealed record TimeFormatListItem(TimeFormat Value, string Display);

    public static class TimeFormatList
    {
        public static readonly IReadOnlyList<TimeFormatListItem> List =
        [
            new(TimeFormat.Default,         "7:49 PM"),
            new(TimeFormat.TwentyFourHour,  "19:49")
        ];
    }

    
}
