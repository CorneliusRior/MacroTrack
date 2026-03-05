using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    public class DiaryConverter
    {
        public int Convert(SqliteConnection conn, TextWriter writer, JsonSerializerOptions jsonOptions)
        {
            using var cmd = new SqliteCommand("SELECT * FROM Diary", conn);
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                string date = reader.GetString(1);
                string time = reader.GetString(2);
                string body = reader.GetString(3);
                body += $"\n\n(Converted from MT5)";
                DiaryAddPayload pl = new(body, Helpers.DTCombine(date, time));
                string json = JsonSerializer.Serialize(pl, jsonOptions);
                writer.WriteLine("Diary.Add");
                writer.WriteLine(json);
                writer.WriteLine();
                count++;
            }
            return count;
        }
    }
}
