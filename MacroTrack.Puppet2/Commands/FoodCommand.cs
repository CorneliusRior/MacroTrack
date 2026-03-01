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
    public class FoodCommand : PuppetCommandBase
    {
        public FoodCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "food";
        public override IReadOnlyList<string> Aliases => new[] { "foodlog", "f", "fooddiary", "foodjournal", "foodentries" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Food"], "Food.<subcommand: Add/Delete/Edit/List>", "Commands for interacting with FoodLog.", Aliases: Aliases, SubCommands: new[] { "Add", "Delete", "Edit", "List" }),
            new(["Food", "Add"], "Food.Add <DateTime Time> <string ItemName> <double Amount> <double Calories> <double Protein> <double Carbs> <double Fat> (string Category) (string Notes)", "Adds food entry."),
            new(["Food", "Add", "Now"], "Food.Add <string ItemName> <double Amount> <double Calories> <double Protein> <double Carbs> <double Fat> (string Category) (string Notes)", "Adds food entry at current time."),
            new(["Food", "Delete"], "Food.Delete <int Id>", "Deletes specified Food Entry."),
            new(["Food", "Edit"], "Food.Edit <int ID> <DateTime time> <string ItemName> <double Amount> <double Calories> <double Protein> <double Carbs> <double Fat> (string Category) (string Notes)", "Sets values for specified entry."),
            new(["Food", "List"], "Food.List", "Lists all Food Entries."),
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(head, args),
                "delete"    => Delete(args),
                "edit"      => Edit(args),
                "list"      => List(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
        }

        public PuppetResult Add(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            FoodEntry entry;
            bool timeNow = false;
            if (head.Count > 2) if (head[2].Equals("now", StringComparison.OrdinalIgnoreCase)) timeNow = true;
            if (args.IsJson())
            {
                if (!timeNow)
                {
                    AddPayload payload = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                    entry = _services.foodLogService.AddEntry(payload.Time, payload.ItemName, payload.Amount, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, payload.Category, payload.Notes);
                }
                else
                {
                    AddNowPayload payload = JsonSerializer.Deserialize<AddNowPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                    entry = _services.foodLogService.AddEntry(DateTime.Now, payload.ItemName, payload.Amount, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, payload.Category, payload.Notes);
                }
            }
            else
            {
                if (!timeNow)
                {
                    DateTime time = args.dateTime(0, "Time");
                    string itemName = args.String(1, "Item Name");
                    double amount = args.Double(2, "Amount");
                    double calories = args.Double(3, "Calories");
                    double protein = args.Double(4, "Protein");
                    double carbs = args.Double(5, "Carbs");
                    double fat = args.Double(6, "Fat");
                    string? category = args.StringOrNull(7, "Category");
                    string? notes = args.StringOrNull(8, "Notes");
                    entry = _services.foodLogService.AddEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
                }
                else
                {
                    DateTime time = DateTime.Now;
                    string itemName = args.String(0, "Item Name");
                    double amount = args.Double(1, "Amount");
                    double calories = args.Double(2, "Calories");
                    double protein = args.Double(3, "Protein");
                    double carbs = args.Double(4, "Carbs");
                    double fat = args.Double(5, "Fat");
                    string? category = args.StringOrNull(6, "Category");
                    string? notes = args.StringOrNull(7, "Notes");
                    entry = _services.foodLogService.AddEntry(time, itemName, amount, calories, protein, carbs, fat, category, notes);
                }
            }
            return PuppetResult.Ok($"Added FoodLog Entry #{entry.Id}");
        }

        public PuppetResult Delete(IReadOnlyList<string> args)
        {
            int id;
            if (args.IsJson())
            {
                DeletePayload payload = JsonSerializer.Deserialize<DeletePayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                id = payload.Id;
            }
            else id = args.Int(0, "Id");
            FoodEntry entry = _services.foodLogService.DeleteEntry(id);
            return PuppetResult.Ok($"Deleted FoodLog Entry #{entry.Id}");
        }

        public PuppetResult Edit(IReadOnlyList<string> args)
        {
            FoodEntry entry;
            // Note that the JSON version does not bother with defaults.
            if (args.IsJson())
            {
                EditPayload payload = JsonSerializer.Deserialize<EditPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.foodLogService.EditEntry(payload.Id, payload.Time, payload.ItemName, payload.Amount, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, payload.Category, payload.Notes);
            }
            else
            {
                int id = args.Int(0, "Id");
                // Get data for what it was before, "r" like "ORiginal".
                FoodEntry r = _services.foodLogService.GetEntry(id);
                DateTime time = args.dateTimeOr(1, "Time", r.Time);
                string itemName = args.StringOrDefault(2, "Item Name", r.ItemName);
                double amount = args.DoubleOr(3, "Amount", r.Amount);
                double calories = args.DoubleOr(4, "Calories", r.Calories);
                double protein = args.DoubleOr(5, "Protein", r.Protein);
                double carbs = args.DoubleOr(6, "Carbs", r.Carbs);
                double fat = args.DoubleOr(7, "Fat", r.Fat);
                string? category = args.StringNullableOrDefault(8, "Category", r.Category);
                string? notes = args.StringNullableOrDefault(9, "Notes", r.Notes);
                entry = _services.foodLogService.EditEntry(id, time, itemName, amount, calories, protein, carbs, fat, category, notes);
            }
            return PuppetResult.Ok($"Edited FoodLog Entry #{entry.Id}");
        }

        public PuppetResult List(IReadOnlyList<string> args)
        {
            List<FoodEntry> foodLog = _services.foodLogService.GetAll();

            StringBuilder sb = new();
            //string returnString = $"Listing all FoodLog Entries.\n";
            sb.AppendLine("Listing all FoodLog Entries.");

            sb.AppendLine(
                $"{"Id:", -8} {"Time:", -19} {"Item Name:", -20} {"Calories:", -9} {"Protein:", -9} {"Carbs:", -9} {"Fat:", -9} {"Category:", -15} {"Notes:", -50}"
            );

            foreach (FoodEntry entry in foodLog)
            {
                sb.AppendLine(
                    $"{$"#{entry.Id}",-8} " + 
                    $"{entry.Time.ToString("yyyy-MM-dd HH:mm:ss"),-19} " +
                    $"{entry.ItemName.Truncate(20),-20} " +
                    $"{entry.Calories.ToString("0.#").Truncate(9),9} " + 
                    $"{entry.Protein.ToString("0.#").Truncate(9),9} " + 
                    $"{entry.Carbs.ToString("0.#").Truncate(9),9} " +
                    $"{entry.Fat.ToString("0.#").Truncate(9),9} " + 
                    $"{(entry.Category ?? "").Truncate(15),-15} " + 
                    $"{(entry.Notes ?? "").Truncate(50),-50} "
                );
            }
            return PuppetResult.Ok(sb.ToString());
        }

        private sealed record AddPayload(DateTime Time, string ItemName, double Amount, double Calories, double Protein, double Carbs, double Fat, string? Category, string? Notes);
        private sealed record AddNowPayload(string ItemName, double Amount, double Calories, double Protein, double Carbs, double Fat, string? Category, string? Notes);
        private sealed record DeletePayload(int Id);
        private sealed record EditPayload(int Id, DateTime Time, string ItemName, double Amount, double Calories, double Protein, double Carbs, double Fat, string? Category, string? Notes);        
    }
}
