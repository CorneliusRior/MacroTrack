using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    internal class TaskConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM DailyTaskDefs", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string name = reader.GetString(1);
                TaskAddPayload pl = new(name, "(Converted from MT5)");
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Task.Add");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }

    public sealed record TaskAddPayload(string Name, string? Description);
}
