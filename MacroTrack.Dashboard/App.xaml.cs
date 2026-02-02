using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Services;
using MacroTrack.Core.Settings;

namespace MacroTrack.Dashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public CoreServices Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Get paths & apply:
            // Database:
            string dbPath = Paths.FindDBPath();
            string connString = $"Data Source ={dbPath}";

            // Logger:
            string logPath = Paths.FindLogPath();
            Paths.DeleteOldLogs(20);
            string logFile = Path.Combine(logPath, $"MTLog_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            var logger = new MTLogger(logFile, this.GetType().Namespace);

            // Settings:
            string settingsPath = Paths.FindSettingsPath();
            var settingsService = new SettingsService(settingsPath);
            logger.UILevel = settingsService.Settings.LogUILevel;
            logger.FileLevel = settingsService.Settings.LogFileLevel;


            // Create context, then CoreServices
            var context = new CoreContext(connString, logger, settingsService);
            Services = new CoreServices(context);

            // Show Window:
            var mainWindow = new MainWindow(Services);
            mainWindow.Show();            
        }
    }

}
