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
    public class GoalCommand : PuppetCommandBase
    {
        public GoalCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "goal";
        public override IReadOnlyList<string> Aliases => new[] { "g", "target", "setup", "goal" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            // We put the activation ones at the bottom to seperate, though when listed it won't look like this :(
            new(["Goal"], "Goal.<subcommand: Add/Delete/Edit/List/Activate/DeleteActivation/GetActive/History>", "Commands for interacting with GoalRegistry and GoalHistory.", Aliases: Aliases, SubCommands: new[] { "Add", "Delete", "Edit", "List", "Activate", "DeleteActivation", "GetActive", "History" }),
            new(["Goal", "Add"], "Goal.Add <string GoalName> <double Calories> <double Protein> <double Carbs> <double Fat> <string GoalType> (string CustomType) (string Notes) (double MinCal) (double MaxCal) (double MinPro) (double MaxPro) (double MinCar) (double MaxCar) (double MinFat) (double MaxFat)", "Adds new goal. GoalTypes are 'None', 'Custom', 'Cut', 'Maintenance', 'Bulk'."),
            new(["Goal", "Delete"], "Goal.Delete <int Id>", "Deletes selected entry."),
            new(["Goal", "Edit"], "Goal.Edit <int Id> <string GoalName> <double Calories> <double Protein> <double Carbs> <double Fat> <string GoalType> (string CustomType) (string Notes) (double MinCal) (double MaxCal) (double MinPro) (double MaxPro) (double MinCar) (double MaxCar) (double MinFat) (double MaxFat)", "Updates specified entry with specified parameters. Mark with '_' to leave unchanged."),
            new(["Goal", "List"], "Goal.List", "Lists all goals."),
            new(["Goal", "Activate"], "Goal.Activate <int Id> (DateTime Date)", "Sets a goal as active at specified date, or today if left blank.", Aliases: new[] { "Activate" }),
            new(["Goal", "DeleteActivation"], "Goal.DeleteActivation <int Id>", "Deletes a GoalActivation from GoalHistory, use this to undo a 'SetActivation' command."),
            new(["Goal", "GetActive"], "Goal.GetActive (DateTime Date)", "Gets the active goal at specified time, or now if left blank.", Aliases: new[] { "GetCurrent", "GetCurrentGoal", "Current" }),
            new(["Goal", "History"], "Goal.History", "Lists all goal activations/changes"),
        ];        

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"               => Add(args),
                "delete"            => Delete(args),
                "edit"              => Edit(args),
                "list"              => List(args),
                "activate"          => ActivateGoal(args),
                "setactivation"     => ActivateGoal(args),
                "deleteactivation"  => DeleteActivation(args),
                "getactive"         => GetActive(args),
                "getcurrent"        => GetActive(args),
                "getcurrentgoal"    => GetActive(args),
                "current"           => GetActive(args),
                "history"           => History(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public override PuppetResult TestJson(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"               => TestAdd(args),
                "delete"            => TestDelete(args),
                "edit"              => TestEdit(args),
                "list"              => PuppetResult.Ok("No Json in this command."),
                "activate"          => TestActive(args),
                "setactivation"     => TestActivateGoal(args),
                "deleteactivation"  => TestDeleteActivation(args),
                "getactive"         => TestActivateGoal(args),
                "getcurrent"        => PuppetResult.Ok("No Json in this command."),
                "getcurrentgoal"    => PuppetResult.Ok("No Json in this command."),
                "current"           => PuppetResult.Ok("No Json in this command."),
                "history"           => PuppetResult.Ok("No Json in this command."),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };            
        }

        public PuppetResult Add(IReadOnlyList<string> args)
        {
            Goal entry;
            if (args.IsJson())
            {
                AddPayload payload = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                GoalType type = payload.GoalType.ToGoalType();
                entry = _services.goalService.AddGoal(payload.GoalName, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, type, payload.CustomType, payload.Notes, payload.MinCal, payload.MaxCal, payload.MinPro, payload.MaxPro, payload.MinCar, payload.MaxCar, payload.MinFat, payload.MaxFat);
            }
            else
            {
                string goalName     = args.String(0, "Name");
                double calories     = args.Double(1, "Calories");
                double protein      = args.Double(2, "Protein");
                double carbs        = args.Double(3, "Carbs");
                double fat          = args.Double(4, "Fat");
                GoalType type       = args.String(5, "GoalType").ToGoalType();
                string? customType  = args.StringOrNull(6, "CustomType");
                string? notes       = args.StringOrNull(7, "Notes");
                double? minCal      = args.DoubleOrNull(8, "MinCal");
                double? maxCal      = args.DoubleOrNull(9, "MaxCal");
                double? minPro      = args.DoubleOrNull(10, "MinPro");
                double? maxPro      = args.DoubleOrNull(11, "MaxPro");
                double? minCar      = args.DoubleOrNull(12, "MinCar");
                double? maxCar      = args.DoubleOrNull(13, "MaxCar");
                double? minFat      = args.DoubleOrNull(14, "MinFat");
                double? maxFat      = args.DoubleOrNull(15, "MaxFat");
                entry = _services.goalService.AddGoal(goalName, calories, protein, carbs, fat, type, customType, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
            }
            return PuppetResult.Ok($"Added Goal #{entry.Id}");
        }

        private PuppetResult TestAdd(IReadOnlyList<string> args)
        {
            try
            {
                AddPayload pl = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                GoalType type = pl.GoalType.ToGoalType();
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult Delete(IReadOnlyList<string> args)
        {
            Goal entry;
            if (args.IsJson())
            {
                DeletePayload payload = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                entry = _services.goalService.DeleteGoal(payload.Id);
            }
            else
            {
                int id = args.Int(0, "Id");
                entry = _services.goalService.DeleteGoal(id);
            }
            return PuppetResult.Ok($"Deleted Goal #{entry.Id}");            
        }

        private PuppetResult TestDelete(IReadOnlyList<string> args)
        {
            try
            {
                DeletePayload pl = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult Edit(IReadOnlyList<string> args)
        {
            Goal entry;
            // JSON version does not bother with defaults.
            if (args.IsJson())
            {
                EditPayload payload = JsonSerializer.Deserialize<EditPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                GoalType type = payload.GoalType.ToGoalType();
                entry = _services.goalService.EditGoal(payload.Id, payload.GoalName, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, type, payload.CustomType, payload.Notes, payload.MinCal, payload.MaxCal, payload.MinPro, payload.MaxPro, payload.MinCar, payload.MaxCar, payload.MinFat, payload.MaxFat);
            }
            else
            {
                int id = args.Int(0, "Id");
                Goal r = _services.goalService.GetGoal(id);
                string goalName     = args.StringOrDefault(1, "Goal Name", r.GoalName);
                double calories     = args.DoubleOr(2, "Calories", r.Calories);
                double protein      = args.DoubleOr(3, "Protein", r.Protein);
                double carbs        = args.DoubleOr(4, "Carbs", r.Carbs);
                double fat          = args.DoubleOr(5, "Fat", r.Fat);
                string? typeString  = args.StringNullableOrDefault(6, "GoalType", null);
                GoalType goalType   = typeString is null ? r.GoalType : typeString.ToGoalType();
                string? customType  = args.StringNullableOrDefault(7, "CustomType", r.CustomType);
                string? notes       = args.StringNullableOrDefault(8, "Notes", r.Notes);
                double? minCal      = args.DoubleOrNullable(9, "MinCal", r.MinCal);
                double? maxCal      = args.DoubleOrNullable(10, "MaxCal", r.MaxCal);
                double? minPro      = args.DoubleOrNullable(11, "MinPro", r.MinPro);
                double? maxPro      = args.DoubleOrNullable(12, "MaxPro", r.MaxPro);
                double? minCar      = args.DoubleOrNullable(13, "MinCar", r.MinCar);
                double? maxCar      = args.DoubleOrNullable(14, "MaxCar", r.MaxCar);
                double? minFat      = args.DoubleOrNullable(15, "MinFat", r.MinFat);
                double? maxFat      = args.DoubleOrNullable(16, "MaxFat", r.MaxFat);
                entry = _services.goalService.EditGoal(id, goalName, calories, protein, carbs, fat, goalType, customType, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
            }
            return PuppetResult.Ok($"Edited Goal #{entry.Id}");
        }

        private PuppetResult TestEdit(IReadOnlyList<string> args)
        {
            try
            {
                EditPayload pl = JsonSerializer.Deserialize<EditPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult History(IReadOnlyList<string> args)
        {
            List<GoalActivation> history = _services.goalService.GetGoalHistory();
            StringBuilder sb = new();
            sb.AppendLine("Printing Goal History:");
            sb.AppendLine(GoalActivation.PrintHeader());
            foreach (GoalActivation ga in history) sb.AppendLine(ga.Print());
            return PuppetResult.Ok(sb.ToString());
        }

        public PuppetResult List(IReadOnlyList<string> args)
        {
            List<Goal> GoalReg = _services.goalService.GetAllGoals();

            StringBuilder sb = new();
            sb.AppendLine("Printing Goal Registry:");
            sb.AppendLine(
                $"{"Id:", -7}" +
                $"{"Name:", -30}" +
                $"{"Type:", -15}" +
                $"{"Cal.:", -7}" +
                $"{"Pro.:", -6}" +
                $"{"Car.:", -6}" +
                $"{"Fat:", -6}" +
                $"{"Min/Max Cal.:", -19}" +
                $"{"Min/Max Pro.:", -17}" +
                $"{"Min/Max Car.:", -17}" +
                $"{"Min/Max Fat.:", -17}" +
                $"{"Notes:", -50}"
            );
            foreach (Goal g in GoalReg) sb.AppendLine(
                $"{$"#{g.Id}",-7}" +
                $"{$"{g.GoalName}",-30}" +
                $"{$"{(g.GoalType == GoalType.Custom ? (g.CustomType ?? "Unnamed custom").Truncate(15) : g.GoalType.ToString().Truncate(15))}",-15}" +
                $"{$"{g.Calories.ToString("0.#").Truncate(7)}", 7}" +
                $"{$"{g.Protein.ToString("0.#").Truncate(6)}", 6}" +
                $"{$"{g.Carbs.ToString("0.#").Truncate(6)}", 6}" +
                $"{$"{g.Fat.ToString("0.#").Truncate(6)}", 6}" +
                $"{$"{(g.MinCal is null ? "-" : g.MinCal.Value.ToString("0.#").Truncate(9))}", 9}/{$"{(g.MaxCal is null ? "-" : g.MaxCal.Value.ToString("0.#").Truncate(9))}",-9}" +
                $"{$"{(g.MinPro is null ? "-" : g.MinPro.Value.ToString("0.#").Truncate(9))}", 8}/{$"{(g.MaxPro is null ? "-" : g.MaxPro.Value.ToString("0.#").Truncate(9))}",-8}" +
                $"{$"{(g.MinCar is null ? "-" : g.MinCar.Value.ToString("0.#").Truncate(9))}", 8}/{$"{(g.MaxCar is null ? "-" : g.MaxCar.Value.ToString("0.#").Truncate(9))}",-8}" +
                $"{$"{(g.MinFat is null ? "-" : g.MinFat.Value.ToString("0.#").Truncate(9))}", 8}/{$"{(g.MaxFat is null ? "-" : g.MaxFat.Value.ToString("0.#").Truncate(9))}",-8}" +
                $"{$"{(g.Notes ?? "-").Truncate(50)}", -50}"
            );
            return PuppetResult.Ok(sb.ToString());
        }

        public PuppetResult ActivateGoal(IReadOnlyList<string> args)
        {
            int id;
            DateTime time;
            if (args.IsJson())
            {
                GoalActivationPayload payload = JsonSerializer.Deserialize<GoalActivationPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                id = payload.Id;
                time = payload.Date ?? DateTime.Today;
            }
            else
            {
                id = args.Int(0, "Id");
                time = args.dateTimeOr(1, "Date", DateTime.Today);
            }
            DateOnly date = DateOnly.FromDateTime(time);
            GoalActivation entry;
            entry = _services.goalService.ActivateGoal(id, date);
            return PuppetResult.Ok($"Activated Goal #{id} for date {time.ToString("yyyy-MM-dd")}, GoalActivation ID = #{entry.Id}");
        }

        private PuppetResult TestActivateGoal(IReadOnlyList<string> args)
        {
            try
            {
                GoalActivationPayload pl = JsonSerializer.Deserialize<GoalActivationPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult DeleteActivation(IReadOnlyList<string> args)
        {
            GoalActivation entry;
            if (args.IsJson())
            {
                // We're just going to reuse the delete one for the goals, same format. Feels wrong but I don't see why it wouldn't work.
                DeletePayload payload = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                entry = _services.goalService.DeleteGoalActivation(payload.Id);
            }
            else
            {
                int id = args.Int(0, "Id");
                entry = _services.goalService.DeleteGoalActivation(id);
            }
            return PuppetResult.Ok($"Deleted GoalActivation #{entry.Id}");
        }

        private PuppetResult TestDeleteActivation(IReadOnlyList<string> args)
        {
            try
            {
                DeletePayload pl = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        public PuppetResult GetActive(IReadOnlyList<string> args)
        {
            DateTime time;
            if (args.IsJson())
            {
                GetActivePayload payload = JsonSerializer.Deserialize<GetActivePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload");
                time = payload.Date ?? DateTime.Today;
            }
            else time = args.dateTimeOr(0, "Date", DateTime.Today);
            DateOnly date = DateOnly.FromDateTime(time);
            GoalActivation? entry = _services.goalService.GetPresentGoal(date);
            if (entry is null) return PuppetResult.Ok($"No Goals activa as of {date.ToString("yyyy-MM-dd")}");
            return PuppetResult.Ok($"Active Goal as of {time.ToString("yyyy-MM-dd")} is Goal #{entry.GoalId}, set by GoalActivation #{entry.Id}");
        }

        private PuppetResult TestActive(IReadOnlyList<string> args)
        {
            try
            {
                GetActivePayload pl = JsonSerializer.Deserialize<GetActivePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
            }
            catch { return PuppetResult.Fail("Invalid JSON payload."); }
            return PuppetResult.Ok("Parsed.");
        }

        private sealed record AddPayload(string GoalName, double Calories, double Protein, double Carbs, double Fat, string GoalType, string? CustomType, string? Notes, double? MinCal, double? MaxCal, double? MinPro, double? MaxPro, double? MinCar, double? MaxCar, double? MinFat, double? MaxFat);
        private sealed record DeletePayload(int Id);
        private sealed record EditPayload(int Id, string GoalName, double Calories, double Protein, double Carbs, double Fat, string GoalType, string? CustomType, string? Notes, double? MinCal, double? MaxCal, double? MinPro, double? MaxPro, double? MinCar, double? MaxCar, double? MinFat, double? MaxFat);
        private sealed record GoalActivationPayload(int Id, DateTime? Date);
        private sealed record GetActivePayload(DateTime? Date);
    }
}
