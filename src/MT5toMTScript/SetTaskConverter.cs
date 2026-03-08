using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    internal class SetTaskConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM DailyTasks", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                // This loop iterates rows. 
                string dateStr = reader.GetString(0);
                DateTime date = DateTime.ParseExact(dateStr, "yy/MM/dd", null, System.Globalization.DateTimeStyles.None);
                //DateTime date = reader.GetDateTime(0); // restructure this? Is string stored as yy/MM/dd - very ambiguous.
                for (int i = 1; i < reader.FieldCount; i++) 
                {
                    // This loop iterates columns. 1 because 0 is Date.
                    // If it is 0 or null, don't do anything:
                    if (reader.IsDBNull(i)) continue;
                    if (reader.GetInt32(i) == 0) continue;
                    int id = int.Parse(reader.GetName(i));
                    TaskSetPayload pl = new(true, id, date);
                    string json = JsonSerializer.Serialize(pl, jsonOptions);
                    writer.WriteLine("Task.Set");
                    writer.WriteLine(json);
                    count++;
                }
            }
            return count;
        }
    }

    public sealed record TaskSetPayload(bool Complete, int Id, DateTime? Date);
}
