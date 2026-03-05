using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    /// <summary>
    /// Converts food
    /// </summary>
    public class FoodConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Food_Log", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string date = reader.GetString(1);
                string time = reader.GetString(2);
                string name = reader.GetString(3);
                double amount = reader.IsDBNull(4) ? 1 : reader.GetDouble(4);
                double calories = reader.GetDouble(5); // Some of these are like 11 decimal places! Sample entries.
                double protein = reader.GetDouble(6);
                double carbs = reader.GetDouble(7);
                double fat = reader.GetDouble(8);
                string notes = reader.IsDBNull(9) ? "" : reader.GetString(9);

                // Convert to civilised format:
                DateTime dateTime = Helpers.DTCombine(date, time);
                calories = Math.Round(calories, 2);

                // Leave our mark:
                notes = string.IsNullOrWhiteSpace(notes) ? "(Converted from MT5)" : notes + $"\n\n(Converted from MT5)";

                FoodAddPayload pl = new(dateTime, name, amount, calories, protein, carbs, fat, null, notes);
                string jason = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Food.Add");
                writer.WriteLine(jason);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }

    public sealed record FoodAddPayload(DateTime Time, string ItemName, double Amount, double Calories, double Protein, double Carbs, double Fat, string? Category, string? Notes);
}
