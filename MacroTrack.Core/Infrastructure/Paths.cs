using System;
using System.Collections.Generic;
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
