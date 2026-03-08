using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    internal class CheatConverter
    {
        public int convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Cheat", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string dateStr = reader.GetString(0);
                DateTime date = DateTime.ParseExact(dateStr, "yy/MM/dd", null, System.Globalization.DateTimeStyles.None);

                CheatSetPayload pl = new(date, true);
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Task.Cheat.Set");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }
}
