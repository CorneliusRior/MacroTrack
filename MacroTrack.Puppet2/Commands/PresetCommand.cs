using DocumentFormat.OpenXml.Office2019.Drawing.SVG;
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
    public class PresetCommand : PuppetCommandBase
    {
        public PresetCommand(CoreServices services, IPuppetContext context) : base(services, context) { }
        public override string Name => "preset";
        public override IReadOnlyList<string> Aliases => new[] { "p", "foodpresets" };
        public override IReadOnlyList<CommandHelp> Help =>
        [
            new(["Preset"], 
                "Preset.<subcommand: Add/Delete/Edit/List", 
                "Commands for interacting with Presets.",
                LongDescription: "Presets contain information about a certain kind of food which allows the user to repeatedly add the same type quickly without much effort, and without having to remember the nutritional information. They can be multiplied as well, stored with the double 'weight', to allow for greater flexibility.",
                Aliases: Aliases,
                SubCommands: new[] { "Add", "Delete", "Edit", "List" }
            ),
            new(["Preset", "Add"], 
                "Preset.Add <string PresetName> <double Calories> <double Protein> <double Carbs> <double Fat> (double Weight) (string Unit) (string Category) (string Notes)", 
                "Adds a new preset."
            ),
            new(["Preset", "Delete"], 
                "Preset.Delete <int Id>", 
                "Deletes specified entry."
            ),
            new(["Preset", "Edit"],
                "Preset.Edit <int Id> <string PresetName> <double Calories> <double Protein> <double Carbs> <double Fat> (double Weight) (string Unit) (string Category) (string Notes)", 
                "Sets values for specified entry. Use '_' to keep previous values."
            ),
            new(["Preset", "List"], 
                "Preset.List (bool Alphabetical)", 
                "Prints all presets. Ordered by ID, or alphabetically by Preset Name if Alphabetical set true"
            ),
        ];

        public override PuppetResult Execute(IReadOnlyList<string> head, IReadOnlyList<string> args)
        {
            if (head.Count < 2) PuppetResult.FailHelp(Help[0], FailHelpType.NoSubCommand);
            return head[1].ToLowerInvariant().Trim() switch
            {
                "add"       => Add(args),
                "delete"    => Delete(args),
                "edit"      => Edit(args),
                "list"      => List(args),
                _ => PuppetResult.Fail($"Unknown subcommand '{Name}.{head[1]}'.")
            };
            throw new NotImplementedException();
        }

        public PuppetResult Add(IReadOnlyList<string> args)
        {
            Preset entry;
            if (args.IsJson())
            {
                AddPayload payload = JsonSerializer.Deserialize<AddPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.presetService.AddEntry(payload.PresetName, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, payload.Weight, payload.Unit, payload.Category, payload.Notes);
            }
            else
            {
                string presetName = args.String(0, "PresetName");
                double calories = args.Double(1, "Calories");
                double protein = args.Double(2, "Protein");
                double carbs = args.Double(3, "Carbs");
                double fat = args.Double(4, "Fat");
                double? weight = args.DoubleOrNull(5, "Weight");
                string? unit = args.StringOrNull(6, "Unit");
                string? category = args.StringOrNull(7, "Category");
                string? notes = args.StringOrNull(8, "Notes");
                entry = _services.presetService.AddEntry(presetName, calories, protein, carbs, fat, weight, unit, category, notes);
            }
            return PuppetResult.Ok($"Added Preset #{entry.Id}");
        }

        public PuppetResult Delete(IReadOnlyList<string> args)
        {
            int id;
            if (args.IsJson())
            {
                DeletePayLoad payload = JsonSerializer.Deserialize<DeletePayLoad>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                id = payload.Id;
            }
            else
            {
                id = args.Int(0, "Id");
            }
            Preset entry = _services.presetService.DeleteEntry(id);
            return PuppetResult.Ok($"Deleted Preset #{entry.Id}");
            throw new NotImplementedException();
        }

        public PuppetResult Edit(IReadOnlyList<string> args)
        {
            Preset entry;
            if (args.IsJson())
            {
                // JSON does not bother with defaults.
                EditPayload payload = JsonSerializer.Deserialize<EditPayload>(args[0]) ?? throw new PuppetUserException("Invalid JSON payload.");
                entry = _services.presetService.EditEntry(payload.Id, payload.PresetName, payload.Calories, payload.Protein, payload.Carbs, payload.Fat, payload.Weight, payload.Unit, payload.Category, payload.Notes);
            }
            else
            {
                int id = args.Int(0, "Id");
                Preset r = _services.presetService.GetEntry(id) ?? throw new PuppetUserException($"Null entry for entry #{id}");
                string presetName = args.StringOrDefault(1, "PresetName", r.PresetName);
                double calories = args.DoubleOr(2, "Calories", r.Calories);
                double protein = args.DoubleOr(3, "Protein", r.Protein);
                double carbs = args.DoubleOr(4, "Carbs", r.Calories);
                double fat = args.DoubleOr(5, "Fat", r.Fat);
                double? weight = args.DoubleOrNullable(6, "Weight", r.Weight);
                string? unit = args.StringNullableOrDefault(7, "Unit", r.Unit);
                string? category = args.StringNullableOrDefault(8, "Category", r.Category);
                string? notes = args.StringNullableOrDefault(9, "Notes", r.Notes);
                entry = _services.presetService.EditEntry(id, presetName, calories, protein, carbs, fat, weight, unit, category, notes);
            }
            return PuppetResult.Ok($"Edited Preset #{entry.Id}");            
        }

        public PuppetResult List(IReadOnlyList<string> args)
        {
            bool alphabetical = args.BoolOr(0, "Sort Alphabetical", false);
            var presetList = _services.presetService.GetAll().OrderBy(p => p.Id);
            if (alphabetical) presetList = presetList.OrderBy(p => p.PresetName);
            StringBuilder sb = new();
            sb.AppendLine("Printing all Presets:");
            sb.AppendLine(Preset.PrintHeader());
            foreach (Preset p in presetList) sb.AppendLine(p.Print());
            return PuppetResult.Ok(sb.ToString());
        }

        private sealed record AddPayload(string PresetName, double Calories, double Protein, double Carbs, double Fat, double? Weight, string? Unit, string? Category, string? Notes);
        private sealed record DeletePayLoad(int Id);
        private sealed record EditPayload(int Id, string PresetName, double Calories, double Protein, double Carbs, double Fat, double? Weight, string? Unit, string? Category, string? Notes);
    }
}
