using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    public class GoalConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Goals", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string name = reader.GetString(1);
                double calories = reader.GetDouble(2);
                double protein = reader.GetDouble(3);
                double carbs = reader.GetDouble(4);
                double fat = reader.GetDouble(5);
                string notes = reader.IsDBNull(6) ? "" : reader.GetString(6);
                string type = reader.IsDBNull(7) ? "None" : reader.GetString(7);
                double? minCal = reader.IsDBNull(8) ? null : reader.GetDouble(8);
                double? maxCal = reader.IsDBNull(9) ? null : reader.GetDouble(9);
                double? minPro = reader.IsDBNull(10) ? null : reader.GetDouble(10);
                double? maxPro = reader.IsDBNull(11) ? null : reader.GetDouble(11);
                double? minCar = reader.IsDBNull(12) ? null : reader.GetDouble(12);
                double? maxCar = reader.IsDBNull(13) ? null : reader.GetDouble(13);
                double? minFat = reader.IsDBNull(14) ? null : reader.GetDouble(14);
                double? maxFat = reader.IsDBNull(15) ? null : reader.GetDouble(15);

                // silly thing with this is that we determined calories as a proportion of calories. Also seems like I found them to nearest gram.
                protein = Math.Round((protein * calories) / 4, 0);
                carbs = Math.Round((carbs * calories) / 4, 0);
                fat = Math.Round((fat * calories) / 9, 0);
                notes = string.IsNullOrWhiteSpace(notes) ? "(Converted from MT5)" : notes + $"\n\n(Converted from MT5)";

                GoalAddPayload pl = new(name, calories, protein, carbs, fat, type, null, notes, minCal, maxCal, minPro, maxPro, minCar, maxCar, minFat, maxFat);
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Goal.Add");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }

    public sealed record GoalAddPayload(string GoalName, double Calories, double Protein, double Carbs, double Fat, string GoalType, string? CustomType, string? Notes, double? MinCal, double? MaxCal, double? MinPro, double? MaxPro, double? MinCar, double? MaxCar, double? MinFat, double? MaxFat);
}
