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
        public override string Usage =>  "Weight.Add/Delete";
        public override string ShortHelp => "Commands for interacting with WeightLog";
        public override string LongHelp => "Add & Delete";

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            // Input can be Execute(head: {"weight", "add/delete"}, args: {80, date})
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(args),
                "delete"    => Delete(args),
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
