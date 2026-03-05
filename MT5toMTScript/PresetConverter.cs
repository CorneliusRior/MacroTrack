using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    public class PresetConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Presets", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string presetName = reader.GetString(1);
                double calories = reader.GetDouble(2);
                double protein = reader.GetDouble(3);
                double carbs = reader.GetDouble(4);
                double fat = reader.GetDouble(5);
                string? category = reader.IsDBNull(6) ? null : reader.GetString(6);
                double? weight = reader.IsDBNull(7) ? null : reader.GetDouble(7);
                string? unit = reader.IsDBNull(8) ? null : reader.GetString(8);

                PresetAddPayload pl = new(presetName, calories, protein, carbs, fat, weight, unit, category, "(Converted from MT5)");
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Preset.Add");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }

    public sealed record PresetAddPayload(string PresetName, double Calories, double Protein, double Carbs, double Fat, double? Weight, string? Unit, string? Category, string? Notes);
}
