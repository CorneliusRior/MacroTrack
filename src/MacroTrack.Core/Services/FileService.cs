using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
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
            if (fileName.IsInvalidFileName()) throw new FileFormatException($"Invalid file name {fileName}, cannot contain the following: {{ '\', '/', ':', '*', '<', '>', '|' }} (and others).");

            // Build destPath and call backup method:
            string destPath = Path.Combine(backupDir, fileName);
            Backup(destPath);
        }        

        /// <summary>
        /// Ensure that we only keep 3 auto daily backups (or more if we change the settings).
        /// </summary>
        /// <param name="dir"></param>
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

        /// <summary>
        /// Backup a destination file and replace it with a different file. Used to restore backups.
        /// </summary>
        /// <param name="sourcePath">Backup file path</param>
        /// <param name="destinationPath">Database path to be overridden after backup.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public void RestoreDB(string sourcePath, string destinationPath)
        {
            // Ensure source exists:
            if (!File.Exists(sourcePath)) throw new FileNotFoundException($"FIle SourcePath='{sourcePath}' does not exist.");

            // Ensure destination directory exists:
            var destDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destDir)) Directory.CreateDirectory(destDir);

            // See if destinationPath exists. If so, back it up, otherwise, do nothing.
            if (File.Exists(destinationPath))
            {
                // Ensure backupDir exists:
                string backupDir = Paths.FindBackupOverriddenDir();
                Directory.CreateDirectory(backupDir);

                // Create file name
                string ovrdName = Path.GetFileNameWithoutExtension(destinationPath);
                string bkupName = $"{ovrdName}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_pre-override.db";
                string bkupPath = Path.Combine(backupDir, bkupName);
                try { _repo.BackupDatabase(destinationPath, bkupPath); }
                catch (Exception ex)
                {
                    Log($"Backup failed! destinationPath='{destinationPath}', bkupPath='{bkupPath}'. No backup was made.", LogLevel.Error, ex);
                    throw;
                }
            }

            // Backup Source to dest.
            try { _repo.BackupDatabase(sourcePath, destinationPath); }
            catch (Exception ex)
            {
                Log($"Backup failed! sourcePath='{sourcePath}', destinationPath='{destinationPath}'. No new file or override.", LogLevel.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Replaces StartupDatabase with a new path so that it will be generated on the next restart. Make sure when ysing this to restart the program immediately afterwards. Places them directly in the data directory.
        /// </summary>
        /// <param name="name"></param>
        public void NewDB(string name)
        {
            string fileName = $"{name.Replace(' ', '_')}";
            if (!fileName.EndsWith(".db", StringComparison.OrdinalIgnoreCase)) fileName += ".db";
            if (fileName.IsInvalidFileName()) throw new FileFormatException($"Invalid file name {fileName}, cannot contain the following: {{ '\', '/', ':', '*', '<', '>', '|' }} (and others).");
            string filePath = Path.Combine(Paths.FindDataDir(), fileName);            
            _settingsService.NewStartupDatabase(filePath);
        }

        public void ExportToExcel(string outputPath)
        {
            _repo.ExportDataBaseToExcel(outputPath);
        }
    }
}
