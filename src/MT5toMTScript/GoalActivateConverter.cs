using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    internal class GoalActivateConverter
    {
        public int Convers(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM GoalHistory", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string date = reader.GetString(1);
                string time = reader.GetString(2);
                int id = reader.GetInt32(3);

                DateTime dateTime = Helpers.DTCombine(date, time);
                GoalActivationPayload pl = new(id, dateTime);
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Goal.Activate");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }
}
