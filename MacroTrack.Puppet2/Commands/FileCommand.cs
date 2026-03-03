using DocumentFormat.OpenXml.InkML;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class FileCommand : PuppetCommandBase
    {
        public FileCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "file";
        public override IReadOnlyList<string> Aliases => new[] { "data" };
        public override IReadOnlyList<CommandHelp> Help => 
        [
            new(["File"],
                "File.<subcommand: >",
                "Commands for interacting with files, backups, loading, &c."),
            new(["File", "Backup"],
                "File.BackUp <string reason> (bool ShowTimeStamp) (string Prefix)",
                "Creates a Backup of the current Database. Reason is required.",
                Example: $"File.Backup \"Transfering to new computer\" true \"\"",
                LongDescription: $"Generates new .db file in AppData/Local/MacroTrack/Backup/MacroTrack (or MacroTrack.debug if in debug mode). Prefix is appended to the start of the file name, and is 'Backup' by default, but you can get rid of it with empty double quotes as shown in example. Timestamp is in 'yyyy-MM-dd_HH-mm-ss' format, Reason is appended to the end of the file name, each seperated by '_'. Spaces in the reason are repalce with '_' also. Characters should only be ones allowed in filenames (i.e. no '/:?*' &c.).\n\nPlease note that this will override any file with the same name. This will likely only be a problem if ShowTimeStamp='false', be careful if you want to retain existing backups.",
                Aliases: new[] { "BackupNow", "BackupManual", "ManualBackup" }),
            new(["File", "OpenBackupDir"],
                "File.OpenBackupDir",
                "Opens the backup directory AppData/Local/MacroTrack/Backup/MacroTrack (or MacroTrack.debug if in debug mode)."),
            new(["File", "Restore"],
                "File.Restore <string SourcePath> <string DestinationPath",
                "Back up Destination Database, then replace destination Database with Source. Used to restore Backups. Restarts program.",
                Example: $"File.Restore \"C:\\Users\\[...]]\\AppData\\Local\\MacroTrack\\backups\\MacroTrack\\auto\\AutoBackupDaily_2026-03-03_19-34-28.db\" \"C:\\Users\\[...]]\\AppData\\Local\\MacroTrack\\data\\MacroTrack.db\""),
            new(["File", "NewDB"],
                "File.NewDB <string Name>",
                "Creates new database with given name.",
                Example: $"`File.NewDB Ronald`, File.NewDB \"Rosies file\"",
                LongDescription: "Sets the startup"),
            new(["File", "SetDB"],
                "File.SetDB <string Path>",
                "Sets string StartUpDatabase, or StartUpDatabaseDebug if in debug mode, then prompts restart. Needs the full path incl. .db.")
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            p("Made it this far.");
            if (head.Count < 2) return PuppetResult.Fail("Subcommand required (this is not how we decided to do these errors but eh...");
            p("Made it past headcount");
            return head[1].ToLowerInvariant().Trim() switch
            {
                "backup"        => Backup(args),
                "backupnow"     => Backup(args),
                "backupmanual"  => Backup(args),
                "manualbackup"  => Backup(args),
                "openbackupdir" => OpenBackupDir(),
                "restore"       => Restore(head, args),
                "newdb"         => NewDB(args),
                "setdb"         => SetDB(head, args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        private PuppetResult Backup(IReadOnlyList<string> args)
        {
            string reason;
            bool addTimeStamp;
            string? prefix;
            if (args.IsJson())
            {
                BackupPayload pl = JsonSerializer.Deserialize<BackupPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                reason = pl.Reason;
                addTimeStamp = pl.ShowTimeStamp;
                prefix = pl.Prefix;
            }
            else
            {
                reason = args.String(0, "Reason");
                addTimeStamp = args.BoolOr(1, "ShowTimeStamp", true);
                prefix = args.StringOr(2, "Prefix", "Backup");
            }
            _services.fileService.BackupManual(reason, addTimeStamp, prefix);
            return PuppetResult.Ok($"Backup sucessful. Should be named \"{(string.IsNullOrWhiteSpace(prefix) ? "" : $"{prefix.Replace(' ', '_')}_")}{(addTimeStamp ? $"{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")}_" : "")}{reason.Replace(' ', '_') + (reason.EndsWith(".db", StringComparison.OrdinalIgnoreCase) ? "" : ".db")}\"");
        }

        private PuppetResult Restore(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            string srcString, dstString;
            if (args.IsJson())
            {
                RestorePayload pl = JsonSerializer.Deserialize<RestorePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                srcString = pl.Source;
                dstString = pl.Destination;
            }
            else
            {
                srcString = args.String(0, "Source String");
                dstString = args.String(1, "Destinaction String");
            }
            _services.fileService.RestoreDB(srcString, dstString);
            return PuppetResult.ForceRestart();
        }

        private PuppetResult OpenBackupDir()
        {
            Paths.OpenBackupDir();
            return PuppetResult.Ok($"Opened {Paths.FindBackupDir()}");
        }

        public PuppetResult NewDB(IReadOnlyList<string> args)
        {
            string name = args.String(0, "Name");
            _services.fileService.NewDB(name);
            return PuppetResult.ForceRestart();
        }

        public PuppetResult SetDB(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            string dbPath;
            if (args.IsJson())
            {
                SetDBPayload pl = JsonSerializer.Deserialize<SetDBPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                dbPath = pl.Path;
            }
            else dbPath = args.String(0, "DBPath");
            _services.SettingsService.SetStartupDatabase(dbPath);
            return PuppetResult.ForceRestart();
            // return PuppetResult.RequestRestart($"Set StartupDatabase, which is now:\n\n'{_services.SettingsService.GetStartupDatabase()}'\n\nApplication requires a restart to apply. Closing application, would you like to restart? (Pressing 'Cancel' will close this message box, new Database will load next time application launches)", "DBPath changed.", $"DBPath set as '{dbPath}', requesting restart.");
        }

        public sealed record BackupPayload(string Reason, bool ShowTimeStamp, string Prefix);
        public sealed record RestorePayload(string Source, string Destination);
        public sealed record SetDBPayload(string Path);
    }
}
