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
    internal class WeightDeleteCommand : PuppetCommandBase
    {
        public WeightDeleteCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "weight.delete";
        public override IReadOnlyList<string> Aliases => new[] { "delete.weight", "weightdelete", "deleteweight" };
        public override string Usage => "weight.delete <int Id>";
        public override string ShortHelp => "Deletes the specified weight entry.";
        public override string LongHelp => "Deletes the specified weight entry. Try weight.list to find IDs";

        public override PuppetResult Execute(IReadOnlyList<string> args)
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
        private sealed record WeightDeletePayload(int Id);
    }
}
