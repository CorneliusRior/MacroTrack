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
    public class WeightCommand : PuppetCommandBase
    {
        public WeightCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "weight";
        public override IReadOnlyList<string> Aliases => new[] { "weightlog", "w" };

        // Attempting to allow help to reach subcommands:
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Weight"], "Weight.<subcommand: add/delete>", "Commands for interacting with WeightLog.", Aliases: Aliases),
            new(["Weight", "Add"], "Weight.Add <double weight> (DateTime time)", "Adds a weight (Kg) to entry at specified time, or current time if not speficied.", "weight.add 80 27-02-2026 20:45:00", Aliases: new[] { "add", "+", "new" }),
            new(["Weight", "Delete"], "Weight.Delete <double Id>", "Deletes the weight entry with specified Id.", "Weight.Delete 50", Aliases: new[] { "add", "-", "remove" })
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            // Input can be Execute(head: {"weight", "add/delete"}, args: {80, date})
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(args),
                "+"         => Add(args),
                "new"       => Add(args),
                "delete"    => Delete(args),
                "-"         => Delete(args),
                "remove"    => Delete(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'")
            };
        }

        private PuppetResult Add(IReadOnlyList<string> args)
        {
            WeightEntry entry;
            if (args.Count == 1 && args[0].TrimStart().StartsWith("{"))
            {
                var payload = JsonSerializer.Deserialize<WeightAddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                DateTime time = payload.Time ?? DateTime.Now;
                entry = _services.weightLogService.AddEntry(time, payload.Weight);
            }
            else
            {
                double weight = args.Double(0, "Weight");
                DateTime time = args.dateTimeOr(1, "Time", DateTime.Now);
                entry = _services.weightLogService.AddEntry(time, weight);
            }
            return PuppetResult.Ok($"Added entry #{entry.Id}");
        }

        private PuppetResult Delete(IReadOnlyList<string> args)
        {
            WeightEntry entry;
            if (args.Count == 1 && args[0].TrimStart().StartsWith("{"))
            {
                var payload = JsonSerializer.Deserialize<WeightDeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                entry = _services.weightLogService.DeleteEntry(payload.Id);
            }
            else
            {
                int id = args.Int(0, "Id");
                entry = _services.weightLogService.DeleteEntry(id);
            }
            return PuppetResult.Ok($"Deleted entry #{entry.Id}");
        }

        private sealed record WeightAddPayload(double Weight, DateTime? Time);
        private sealed record WeightDeletePayload(int Id);
    }
}
