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
            new(["File", "BackUpTest"],
                "File.BackUpTest",
                "Testing Backup method in FileRepo. Will/should probably delete this.")
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            p("Made it this far.");
            if (head.Count < 2) return PuppetResult.Fail("Subcommand required (this is not how we decided to do these errors but eh...");
            p("Made it past headcount");
            return head[1].ToLowerInvariant().Trim() switch
            {
                "backuptest"    => BackUpTest(),
                "backup"        => Backup(args),
                "backupnow"     => Backup(args),
                "backupmanual"  => Backup(args),
                "manualbackup"  => Backup(args),
                "openbackupdir" => OpenBackupDir(),
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

        private PuppetResult BackUpTest()
        {
            _services.fileService.TestBackup();
            return PuppetResult.Ok("Should have executed, take a look.");
        }

        private PuppetResult OpenBackupDir()
        {
            Paths.OpenBackupDir();
            return PuppetResult.Ok($"Opened {Paths.FindBackupDir()}");
        }

        public sealed record BackupPayload(string Reason, bool ShowTimeStamp, string Prefix);
    }
}
