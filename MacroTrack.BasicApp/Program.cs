using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;
using System.Windows.Forms.DataVisualization.Charting;

namespace MacroTrack.BasicApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        

        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            /*
            var settingsPath = FindSettingsPath();
            var settingsService = new SettingsService(settingsPath);

            var dbPath = FindDBPath();
            var connString = $"Data Source={dbPath}";

            var logPath = FindLogPath();
            string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            DeleteOldLogs(20);

            var _logger = new MTLogger(logFile);
            _logger.UILevel = settingsService.Settings.LogUILevel;
            _logger.FileLevel = settingsService.Settings.LogFileLevel;

            var _context = new CoreContext(connString, _logger, settingsService);
            var _services = new CoreServices(_context);
            */

            // Get paths & apply:
            // Database:
            string dbPath = Paths.FindDBPath();
            string connString = $"Data Source ={dbPath}";

            // Logger:
            string logPath = Paths.FindLogPath();
            string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            var logger = new MTLogger(logFile, "BasicApp");

            // Settings:
            string settingsPath = Paths.FindSettingsPath();
            var settingsService = new SettingsService(settingsPath);
            logger.UILevel = settingsService.Settings.LogUILevel;
            logger.FileLevel = settingsService.Settings.LogFileLevel;


            // Create context, then CoreServices
            var context = new CoreContext(connString, logger, settingsService);
            var Services = new CoreServices(context);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(Services));
        }

        /*
        static string FindAppDataDir()
        {
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(baseDir, "MacroTrack");
        }

        static string FindDBPath()
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

        static string FindLogPath()
        {
            var dir = Path.Combine(FindAppDataDir(), "logs");
            Directory.CreateDirectory(dir);
            return dir;
        }

        static void DeleteOldLogs(int amount)
        {
            var dir = FindLogPath();
            var files = new DirectoryInfo(dir).GetFiles("MTLog_*.txt").OrderByDescending(f => f.CreationTime).ToList();
            foreach (var f in files.Skip(amount)) f.Delete();
        }

        static string FindSettingsPath()
        {
            var dir = Path.Combine(FindAppDataDir(), "config");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "settings.json");
        }
        */
    }
}