using DocumentFormat.OpenXml.Wordprocessing;
using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class DiaryCommand : PuppetCommandBase
    {
        public DiaryCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "diary";
        public override IReadOnlyList<string> Aliases => new[] { "journal", "d" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Diary"], "Diary.<subcommand: Add/Delete/Edit/List/View>", "Commands for interacting with Diary.", Aliases: Aliases),
            new(["Diary", "Add"], "Diary.Add <string Body> (time)", "Adds new Diary Entry."),
            new(["Diary", "Delete"], "Diary.Delete <int Id>", "Deletes specified diary entry."),
            new(["Diary", "Edit"], "Diary.Edit <string Body> (string EditNotes)", "Replaces the body of specified Diary Entry with appended notes.", $"Diary.Edit \"Text placed here replaces the existing DiaryEntry body, you can copy and paste it here, then edit.\" \"Editnotes are appended to the end of the body, after a timestamp.\""),
            new(["Diary", "List"], "Diary.List (bool truncated = true)", "Lists all Diary entries", LongDescription: "Lists all Diary entries in the database. If truncated is set to true (default), will print the ID, date, and first 100 characters of the body. If set to false, will print ID, date and time, and the full text of the body."),
            new(["Diary", "View"], "Diary.View <int Id>", "Prints a single Diary Entry.")
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(args),
                "delete"    => Delete(args),
                "edit"      => Edit(args),
                "list"      => List(args),
                "view"      => View(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        private PuppetResult Add(IReadOnlyList<string> args)
        {
            DiaryEntry entry;
            if (args.IsJson())
            {
                AddPayload payload = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.diaryService.AddEntryAtTime(payload.Body, payload.Time ?? DateTime.Now);
            }
            else
            {
                string body = args.String(0, "Body");
                DateTime time = args.dateTimeOr(1, "Time", DateTime.Now);
                entry = _services.diaryService.AddEntryAtTime(body, time);
            }
            return PuppetResult.Ok($"Added Diary Entry #{entry.Id}");
        }

        private PuppetResult Delete(IReadOnlyList<string> args)
        {
            DiaryEntry entry;
            if (args.IsJson())
            {
                DeletePayload payload = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.diaryService.DeleteEntry(payload.Id);
            }
            else
            {
                int Id = args.Int(0, "Id");
                entry = _services.diaryService.DeleteEntry(Id);
            }
            return PuppetResult.Ok($"Deleted Diary entry #{entry.Id}");
        }

        private PuppetResult Edit(IReadOnlyList<string> args)
        {
            DiaryEntry entry;
            if (args.IsJson())
            {
                EditPayload payload = JsonSerializer.Deserialize<EditPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.diaryService.EditEntry(payload.Id, payload.Body, payload.EditNotes ?? "");
            }
            else
            {
                int id = args.Int(0, "Id");
                DiaryEntry r = _services.diaryService.GetEntry(id);
                string body = args.StringOrDefault(1, "Body", r.Body);
                string editNotes = args.StringOr(2, "Edit Notes", "");
                entry = _services.diaryService.EditEntry(id, body, editNotes);
            }
            return PuppetResult.Ok($"Edited Diary entry #{entry.Id}");
        }

        private PuppetResult List(IReadOnlyList<string> args)
        {
            bool truncate;
            if (args.IsJson())
            {
                ListPayload payload = JsonSerializer.Deserialize<ListPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                truncate = payload.Truncate;
            }
            else truncate = args.BoolOr(0, "Truncate", true);

            // usually I would define this as List<DiaryEntry>, but then it wouldn't let me do OrderBy();
            var diary = _services.diaryService.GetAll().OrderBy(e => e.Time);            

            StringBuilder sb = new();
            sb.AppendLine($"All Diary Entries (truncate = '{truncate}':");

            if (truncate)
            {
                sb.AppendLine($"{"ID:", -7} {"Date:", -12} Body:");
                foreach (DiaryEntry entry in diary)
                {
                    sb.AppendLine($"{$"#{entry.Id}", -7} {entry.Time.ToString("yyyy-MM-dd:"), -12} {entry.Body.Truncate(100)}");
                }
            }
            else
            {
                foreach (DiaryEntry entry in diary)
                {
                    sb.AppendLine($"\n/--- #{entry.Id} {entry.Time.ToString("(yyyy-MM-dd HH:mm:ss)"), -21} {new string('-', 100)}/\n{entry.Body}");
                }
            }
            return PuppetResult.Ok(sb.ToString());
        }

        private PuppetResult View(IReadOnlyList<string> args)
        {
            int id;
            if (args.IsJson())
            {
                ViewPayload payload = JsonSerializer.Deserialize<ViewPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                id = payload.Id;
            }
            else id = args.Int(0, "Id");
            DiaryEntry entry = _services.diaryService.GetEntry(id);
            return PuppetResult.Ok($"#{entry.Id} {entry.Time.ToString("(yyyy-MM-dd HH:mm:ss):"),-21}\n{entry.Body}");
        }

        // JSON payloads:
        private sealed record AddPayload(string Body, DateTime? Time);
        private sealed record DeletePayload(int Id);
        private sealed record EditPayload(int Id, string Body, string EditNotes);
        private sealed record ListPayload(bool Truncate);
        private sealed record ViewPayload(int Id);
    }
}
