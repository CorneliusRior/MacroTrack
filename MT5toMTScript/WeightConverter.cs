using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    internal class WeightConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Weight_Log", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string date = reader.GetString(1);
                string time = reader.GetString(2);
                double weight = reader.GetDouble(3);

                DateTime dateTime = Helpers.DTCombine(date, time);
                weight = Math.Round(weight, 2);

                WeightAddPayload pl = new(weight, dateTime);
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Weight.Add");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }
}
