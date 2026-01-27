using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;

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

            var root = FindSolutionRoot();
            
            var dbPath = FindDBPath();
            var connString = $"Data Source={dbPath}";

            var logPath = FindLogPath();
            string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            DeleteOldLogs(20);

            var _logger = new MTLogger(logFile);
            
            var _services = new CoreServices(connString, _logger);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(_services));
        }

        static string FindSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Data"))) dir = dir.Parent;
            if (dir == null) throw new DirectoryNotFoundException("No");
            return dir.FullName;
        }

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
    }
}