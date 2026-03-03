using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Infrastructure
{
    /// <summary>
    /// Set of methods for finding the paths for data &c. Also automatically switches between User data and debug data.
    /// </summary>
    public static class Paths
    {
        public static string FindAppDataDir()
        {
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(baseDir, "MacroTrack");
        }

        /// <summary>
        /// Returns the backup folder. Note that this returns the basedir, which will contain MacrkTrack.Debug and MacroTrack(.release). Use FindBackupDir for that.
        /// </summary>
        public static string FindBackupBaseDir() => Path.Combine(FindAppDataDir(), "backups");
        
        /// <summary>
        /// Returns the backup directory, taking account of whether you're in debug mode or not.
        /// </summary>
        public static string FindBackupDir()
        {
            string dirName;
            #if DEBUG
            dirName = "MacroTrack.debug";
            #else
            dirName = "MacroTrack";
            #endif
            return Path.Combine(FindBackupBaseDir(), dirName);
        }
        public static string FindBackupAutoDir() => Path.Combine(FindBackupDir(), "auto");
        public static string FindBackupManualDir() => Path.Combine(FindBackupDir(), "manual");

        /// <summary>
        /// Returns the data Directory (default).
        /// </summary>
        public static string FindDataDir() => Path.Combine(FindAppDataDir(), "data");

        /// <summary>
        /// This is added to give functionality to "Open Data File" in File.xaml in settings. Given that we are very shortly going to be changing a lot of the fundamentals here it might be prudent to note that you don't need to cling onto this too much.
        /// </summary>
        public static void OpenAppDataDir() => Process.Start(new ProcessStartInfo
        {
            FileName = FindAppDataDir(),
            UseShellExecute = true
        });

        public static void OpenBackupDir() => Process.Start(new ProcessStartInfo
        {
            FileName = FindBackupDir(),
            UseShellExecute = true
        });

        public static void OpenDataDir() => Process.Start(new ProcessStartInfo 
        { 
            FileName = FindDataDir(),
            UseShellExecute = true,
        });

        public static string FindDBPath()
        {
            var dir = Path.Combine(FindAppDataDir(), "data");
            Directory.CreateDirectory(dir);
            string fileName;

            #if DEBUG
            fileName = "MacroTrack.debug.db";
            #else
            fileName = "MacroTrack.db";
            #endif

            return Path.Combine(dir, fileName);
        }

        public static string FindLogPath()
        {
            var dir = Path.Combine(FindAppDataDir(), "logs");
            Directory.CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// Deletes logs if there are too may.
        /// </summary>
        /// <param name="amount"></param>
        /// <remarks>I'm not actually sure about having it here, should be called by something in core rather than applications imo.</remarks>
        public static void DeleteOldLogs(int amount)
        {
            var dir = FindLogPath();
            var files = new DirectoryInfo(dir).GetFiles("MTLog_*.txt").OrderByDescending(f => f.CreationTime).ToList();
            foreach (var f in files.Skip(amount)) f.Delete();
        }

        public static string FindSettingsPath()
        {
            var dir = Path.Combine(FindAppDataDir(), "config");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "settings.json");
        }        
    }
}
