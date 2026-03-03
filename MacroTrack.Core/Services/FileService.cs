using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Repositories;
using MacroTrack.Core.Settings;

namespace MacroTrack.Core.Services
{
    /// <summary>
    /// Responsible for general file side things, backups, exports, imports, &c., was originally called "ExportService" for ExportToExcel();
    /// </summary>
    public class FileService : ServiceBase
    {
        private readonly FileRepo _repo;
        private readonly SettingsService _settingsService;

        public FileService(FileRepo repo, CoreContext ctx) : base(ctx)
        {
            _repo = repo;
            _settingsService = ctx.Settings;
        }

        /// <summary>
        /// Test of backup system, delete if you find that this hasn't been deleted:
        /// </summary>
        public void TestBackup()
        {
            /*
            string sourceString = Paths.FindDBPath();
            string destString = Path.Combine(Paths.FindDataDir(), "test_backup.db");
            LogVars(new { sourceString, destString }, "These were defined just now, backing up now:");
            _repo.BackupDatabase(Paths.FindDBPath(), Path.Combine(Paths.FindDataDir(), "test_backup.db"));
            Log("Affirming that we got here");*/
        }

        public void Backup(string destPath)
        {
            string srcPath = _settingsService.GetStartupDatabase() ?? Paths.FindDBPath();

            // Try to Backup:
            try { _repo.BackupDatabase(srcPath, destPath); }
            catch (Exception ex)
            {
                Log($"Backup failed! srcPath='{srcPath}', destPath='{destPath}'. No backup was made.", LogLevel.Error, ex);
                throw;
            }
        }

        public void BackupAutoDaily()
        {
            // Determine if we're supposed to back up:
            if (!_settingsService.Settings.BackupDailyAuto) return;
            DateTime last = _settingsService.Settings.BackupDailyLastDate;
            if (last.Date == DateTime.Today) return;

            // Define backupDir string and ensure:
            string backupDir = Paths.FindBackupAutoDir();
            Directory.CreateDirectory(backupDir);

            // Build file name:
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"AutoBackupDaily_{timeStamp}.db";

            // Build destPath and call backup method:
            string destPath = Path.Combine(backupDir, fileName);
            Backup(destPath);

            // Update settings & enforce retention:
            _settingsService.Settings.BackupDailyLastDate = DateTime.Today;
            _settingsService.Save();
            EnforceAutoRetention(backupDir);
        }

        public void BackupManual(string reason, bool addTimeStamp = true, string prefix = "Backup")
        {
            // Define backupDir string and ensure:
            string backupDir = Paths.FindBackupManualDir();        
            Directory.CreateDirectory(backupDir);

            // Build file name:
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"{(string.IsNullOrWhiteSpace(prefix) ? "" : $"{prefix.Replace(' ', '_')}_" )}{(addTimeStamp ? $"{timeStamp}_" : "")}{reason.Replace(' ', '_')}";
            if (!fileName.EndsWith(".db", StringComparison.OrdinalIgnoreCase)) fileName += ".db";

            // Build destPath and call backup method:
            string destPath = Path.Combine(backupDir, fileName);
            Backup(destPath);
        }        

        private void EnforceAutoRetention(string dir)
        {
            // If you want to make other kinds of autobackup, like monthly or yearly, add an argument string prefix, equal to the start (e.g. "AutoBackupDaily"), and add files.StartsWith(prefix); If you want differing retention amounts, add that in settings, and add that as an amount as well.
            int retain = Math.Max(0, _settingsService.Settings.BackupDailyRetentionCount);
            if (retain == 0) return;

            var files = Directory.GetFiles(dir, "*.db").OrderByDescending(f => f).ToList();
            if (files.Count <= retain) return;

            foreach(var f in files.Skip(retain))
            {
                try { File.Delete(f); }
                catch (Exception ex)
                {
                    Log($"Failed to delete old auto backup {f}.", LogLevel.Warning, ex);
                    throw;
                }
            }
        }

        public void ExportToExcel(string outputPath)
        {
            _repo.ExportDataBaseToExcel(outputPath);
        }
    }
}
