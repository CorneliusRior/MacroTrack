using MacroTrack.Core.DataModels;
using MacroTrack.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MacroTrack.Core.Settings
{
    public class SettingsService
    {
        public IMTLogger Logger;

        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public AppSettings Settings { get; private set; }

        public SettingsService(string filePath, IMTLogger logger)
        {
            _filePath = filePath;
            Logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Settings = Load();
        }

        private AppSettings Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions) ?? new AppSettings();
        }

        public void Save()
        {
            Save(Settings);
        }

        private void Save(AppSettings settings)
        {
            Log();
            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(_filePath, json);
            Settings = settings;
        }

        public void Set(AppSettings settings)
        {
            Log();
            Save(settings);
        }

        public void Apply(AppSettings settings)
        {
            // Apply every parameter which wouldn't be UI driven.
            Logger.UILevel = settings.LogUILevel;
            Logger.FileLevel = settings.LogFileLevel;
        }

        private void Log(string message = "Called", LogLevel level = LogLevel.Debug, Exception? ex = null, [CallerMemberName] string caller = "")
        {
            Logger.Log(this, caller, level, message, ex);
        }

        // File/Backup methods:
        /// <summary>
        /// This is to get that and ensure seperation w/ debug.
        /// </summary>
        public string? GetStartupDatabase()
        {
            #if DEBUG
            return Settings.StartupDatabaseDebug;
            #else
            return Settings.StartupDatabasel
            #endif
        }

        public void SetBackupDailyLastDate(DateTime date)
        {
            date = date.Date;
            Settings.BackupDailyLastDate = date;

        }

        public string GetDTFormatShortString()
        {
            return DTFormatSList.FormatByValue.TryGetValue(Settings.DTFormatShort, out var fmt) ? fmt : "yyyy-MM-dd HH:mm";
        }

        public string GetDTFormatLongString()
        {
            return DTFormatLList.FormatByValue.TryGetValue(Settings.DTFormatLong, out var fmt) ? fmt : "dddd, d MMMM, yyyy, H:mm:ss tt";
        }

        public string GetLongDateTimeString(bool timeBeforeDate = false, bool includeSeconds = true, string timeSeperator = ":", string? dateTimeSeperator = null)
        {
            string date = Settings.DFormatLong.GetFormatString();
            string time = Settings.TimeFormat.GetFormatString(includeSeconds, timeSeperator);
            string sep = dateTimeSeperator == null ? Settings.DateTimeSeperator : dateTimeSeperator;
            if (timeBeforeDate) return $"{time}{sep}{date}";
            else return $"{date}{sep}{time}";
        }

        public string GetShortDateTimeString(bool timeBeforeDate = false, bool includeSeconds = true, string dateSeperator = "/", string timeSeperator = ":", string? dateTimeSeperator = null)
        {
            string date = Settings.DFormatShort.GetFormatString(dateSeperator);
            string time = Settings.TimeFormat.GetFormatString(includeSeconds, timeSeperator);
            string sep = dateTimeSeperator == null ? Settings.DateTimeSeperator : dateTimeSeperator;
            if (timeBeforeDate) return $"{time}{sep}{date}";
            else return $"{date}{sep}{time}";
        }

        public string GetLongDateString()
        {
            return Settings.DFormatLong.GetFormatString();
        }

        public string GetShortDateString(string? dateSeperator = null)
        {            
            return Settings.DFormatShort.GetFormatString(dateSeperator ?? Settings.DateSeperator);
        }

        public string GetTimeStringMinutes(string? timeSeperator = null)
        {
            return Settings.TimeFormat.GetFormatString(false, timeSeperator ?? Settings.TimeSeperator);
        }

        public string GetTimeStringSeconds(string? timeSeperator = null)
        {
            return Settings.TimeFormat.GetFormatString(true, timeSeperator ?? Settings.TimeSeperator);
        }

        public string GetDateTimeString(bool longDate = false, bool showMinutes = true, bool showSeconds = true)
        {
            if (longDate)
            {
                if (!showMinutes) return Settings.DFormatLong.GetFormatString();
                else return GetLongDateTimeString(false, showSeconds);
            }
            else // (shortDate:)
            {
                if (!showMinutes) return Settings.DFormatShort.GetFormatString(Settings.DateSeperator);
                else return GetShortDateTimeString(false, showSeconds);
            }
        }

        /// <summary>
        /// Resturns a string showing the time depicted in TimePeriod. Will only display time if the times are not midnight, and only show one date if the period is only 1 day long or less. 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="longFormat"></param>
        /// <param name="inter"></param>
        /// <param name="pre"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public string TimePeriodToString(TimePeriod period, bool longFormat = true, string inter = " - ", string pre = "", string post = "")
        {
            TimeSpan span = period.GetTimeSpan();
            DateTime startTime = period.StartTime;
            DateTime endTime = period.EndTime;

            if (span == TimeSpan.FromDays(1) && startTime.TimeOfDay == TimeSpan.Zero)
            {   // Period is a single day, return a string only showing one date             
                if (longFormat) return startTime.ToString(GetLongDateString());
                return startTime.ToString(GetShortDateString());                
            }
            if (startTime.Date == endTime.Date)
            {   // This period takes place within the one day, return a string with one date, time to time.
                if (longFormat) return $"{startTime.ToString(GetLongDateString())}, from {startTime.ToString(GetTimeStringMinutes())} to {endTime.ToString(GetTimeStringMinutes())}";
                return $"{startTime.ToString(GetShortDateString())}, from {startTime.ToString(GetTimeStringMinutes())} to {endTime.ToString(GetTimeStringMinutes())}";
            }
            if (period.StartTime.TimeOfDay == TimeSpan.Zero && period.EndTime.TimeOfDay == TimeSpan.Zero)
            {   // Period is between distinct days, only return dates:
                if (longFormat) return period.ToString(GetLongDateString());
                return period.ToString(GetShortDateString());
            }
            // Period is between multiple dates and times, return dates and times:
            if (longFormat) return period.ToString(GetLongDateTimeString());
            return period.ToString(GetShortDateTimeString());


        }
    }
}
