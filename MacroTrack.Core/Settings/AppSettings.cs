using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MacroTrack.Core.Logging;

namespace MacroTrack.Core.Settings
{
    /// <summary>
    /// Settings for use in MacroTrack apps. This is used throughout the entire solution, not just one app or another.
    /// </summary>
    public sealed class AppSettings
    {
        // When adding a new setting, be sure to put it both here and in the Clone() method at the bottom.
        public string? StartupDatabase { get; set; } = null; // null means use Paths default. Note that if we use "reset to default" in settings, this will reset to null! I won't bother doing protections for that right now.
        public string? StartupDatabaseDebug { get; set; } = null;
        public bool BackupDailyAuto { get; set; } = true;
        public DateTime BackupDailyLastDate { get; set; } // Will just be DateTime.MinValue if not set. Potential vulnerability: if you went back in time to around the birth of Christ, it wouldn't back up on the first day.
        public int BackupDailyRetentionCount { get; set; } = 3; // How many daily auto backups we have.
        public string Theme { get; set; } = "Light";
        public string ThemeCustomForeground { get; set; } = "#FFFFFF00";
        public string GraphColorPro { get; set; } = "#FFFF0000";
        public string GraphColorCar { get; set; } = "#FF0000FF";
        public string GraphColorFat { get; set; } = "#FFFFFF00";
        public bool LogInitMessages { get; set; } = true;
        public int LogRetainAmount { get; set; } = 20;
        public LogLevel LogUILevel { get; set; } = LogLevel.Warning;
        public LogLevel LogFileLevel { get; set; } = LogLevel.Debug;
        public int HistoryLength { get; set; } = 3; // How many days are shown in food history.
        public bool HistoryShowFuture { get; set; } = true; // Whether entries with timestamp after today are shown.
        public int CalGraphLength { get; set; } = 30; // days, this being a month.
        public int WeightMode { get; set; } = 0; // 0: kg, 1: lbs, 2: st
        public WeightFormat WeightFormat { get; set; } = WeightFormat.Kg;
        public bool WeightGraphShowTrendLine { get; set; } = true;
        public bool WeightGraphShowTrendError { get; set; } = true;
        public int WeightGraphLength { get; set; } = 30; // days, this being a month.
        public bool ClockInLongFormat { get; set; } = true;
        public bool DTPInLongFormat { get; set; } = true;
        public DTFormatS DTFormatShort { get; set; } = DTFormatS.Default; // Default, 2026/2/7 - 7:42 PM
        public DTFormatL DTFormatLong { get; set; } = DTFormatL.Default;
        public DFormatS DFormatShort { get; set; } = DFormatS.ymd;
        public DFormatL DFormatLong { get; set; } = DFormatL.dayDateMonthYear;
        public TimeFormat TimeFormat { get; set; } = TimeFormat.Default;
        public string DateSeperator { get; set; } = "/";
        public string TimeSeperator { get; set; } = ":";
        public string DateTimeSeperator { get; set; } = " ";
        public bool TaskShowInactive { get; set; } = false;
        public bool TaskShowActive { get; set; } = true;
        public bool DiaryTimeIsLongFormat { get; set; } = true;
        public bool DiaryTimeIncludesTime { get; set; } = true;
        public bool DiaryTimeIncludesSeconds { get; set; } = true;
        public bool GoalPctOfCalories { get; set; } = true; // Whether the "%" you get in goal view displays proportion of calories or mass. Also apply this to donut charts!
        public GraphSettings GraphSettings { get; set; } = new GraphSettings();

        public AppSettings Clone()
        {
            return new AppSettings
            {
                StartupDatabase = this.StartupDatabase,
                StartupDatabaseDebug = this.StartupDatabaseDebug,
                BackupDailyAuto = this.BackupDailyAuto,
                BackupDailyLastDate = this.BackupDailyLastDate,
                BackupDailyRetentionCount = this.BackupDailyRetentionCount,
                Theme = this.Theme,
                LogInitMessages = this.LogInitMessages,
                LogRetainAmount = this.LogRetainAmount,
                LogFileLevel = this.LogFileLevel,
                LogUILevel = this.LogUILevel,
                HistoryLength = this.HistoryLength,
                HistoryShowFuture = this.HistoryShowFuture,
                WeightMode = this.WeightMode,
                WeightFormat = this.WeightFormat,
                WeightGraphLength = this.WeightGraphLength,
                WeightGraphShowTrendLine = this.WeightGraphShowTrendLine,
                WeightGraphShowTrendError = this.WeightGraphShowTrendError,
                ClockInLongFormat = this.ClockInLongFormat,
                DTPInLongFormat = this.DTPInLongFormat,
                DTFormatShort = this.DTFormatShort,
                DTFormatLong = this.DTFormatLong,
                DFormatShort = this.DFormatShort,
                DFormatLong = this.DFormatLong,
                TimeFormat = this.TimeFormat,
                DateSeperator = this.DateSeperator,
                TimeSeperator = this.TimeSeperator,
                DateTimeSeperator = this.DateTimeSeperator,
                TaskShowInactive = this.TaskShowInactive,
                TaskShowActive = this.TaskShowActive,
                DiaryTimeIsLongFormat = this.DiaryTimeIsLongFormat,
                DiaryTimeIncludesTime = this.DiaryTimeIncludesTime,
                DiaryTimeIncludesSeconds = this.DiaryTimeIncludesSeconds,
                GoalPctOfCalories = this.GoalPctOfCalories,
                GraphSettings = this.GraphSettings.Clone()
            };
        }

    }

    
}
