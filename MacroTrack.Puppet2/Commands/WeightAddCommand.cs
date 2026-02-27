using MacroTrack.Core.Models;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2.Commands
{
    public class WeightAddCommand : PuppetCommandBase
    {
        public WeightAddCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "weight.add";
        public override IReadOnlyList<string> Aliases => new[] { "add.weight", "weightadd", "addweight" };
        public override string Usage => "weight.add <double weight> (dateTime date)";
        public override string ShortHelp => "Adds weight entry to weight log (Kg).";
        public override string LongHelp => "Adds specified weight in Kg to the weight log at the specified date.";

        public override PuppetResult Execute(IReadOnlyList<string> args)
        {
            double weight = args.Double(0, "Weight");
            DateTime time = args.dateTimeOr(1, "Time", DateTime.Now);
            WeightEntry entry = _services.weightLogService.AddEntry(time, weight);
            return PuppetResult.Ok($"Added entry #{entry.Id}");
        }
    }
}
