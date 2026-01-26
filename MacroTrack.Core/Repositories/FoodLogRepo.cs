namespace MacroTrack.Core.Repositories;

using MacroTrack.Core.Models;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

public class FoodLogRepo
{
    private readonly string _connectionString;
    public event EventHandler<string> RequestPrint;

    public FoodLogRepo(string connectionString)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        CREATE TABLE IF NOT EXISTS FoodLog (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Time TEXT NOT NULL,
            ItemName TEXT NOT NULL,
            Amount DOUBLE NOT NULL,
            Calories DOUBLE NOT NULL,
            Protein DOUBLE NOT NULL,
            Carbs DOUBLE NOT NULL,
            Fat DOUBLE NOT NULL,
            Category TEXT,
            Notes TEXT
        );
        ";

        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    // new
    public void AddEntry(FoodEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        INSERT INTO FoodLog (Time, ItemName, Amount, Calories, Protein, Carbs, Fat, Category, Notes) 
            VALUES ($time, $itemname, $amount, $calories, $protein, $carbs, $fat, $category, $notes)
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$time", entry.Time);
        cmd.Parameters.AddWithValue("$itemname", entry.ItemName);
        cmd.Parameters.AddWithValue("$amount", entry.Amount);
        cmd.Parameters.AddWithValue("$calories", entry.Calories);
        cmd.Parameters.AddWithValue("$protein", entry.Protein);
        cmd.Parameters.AddWithValue("$carbs", entry.Carbs);
        cmd.Parameters.AddWithValue("$fat", entry.Fat);
        cmd.Parameters.AddWithValue("$category", entry.Category ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", entry.Notes ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    // Load
    public FoodEntry? GetEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM FoodLog WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;

        string? category = reader.IsDBNull(8) ? null : reader.GetString(8);
        string? notes = reader.IsDBNull(9) ? null : reader.GetString(9);
        
        return new FoodEntry(
            reader.GetInt32(0),
            reader.GetDateTime(1),
            reader.GetString(2),
            reader.GetDouble(3),
            reader.GetDouble(4),
            reader.GetDouble(5),
            reader.GetDouble(6),
            reader.GetDouble(7),
            category,
            notes
        );
    }

    // Load all
    public List<FoodEntry>? GetAll()
    {
        var FoodList = new List<FoodEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM FoodLog";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string? category = reader.IsDBNull(8) ? null : reader.GetString(8);
            string notes = reader.IsDBNull(9) ? "" : reader.GetString(9);
            
            FoodList.Add(
                new FoodEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetString(2),
                    reader.GetDouble(3),
                    reader.GetDouble(4),
                    reader.GetDouble(5),
                    reader.GetDouble(6),
                    reader.GetDouble(7),
                    category,
                    notes
                )
            );
        }
        
        if (FoodList.Count == 0) return null;
        return FoodList;
    }

    // Load all entries for a specific category:
    public List<FoodEntry>? GetAllCategory(string? category)
    {
        var foodList = new List<FoodEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql;
        SqliteCommand cmd;

        if (category is null)
        {
            // Must use IS NULL explicitly; `= NULL` never matches in SQL.
            sql = "SELECT * FROM FoodLog WHERE Category IS NULL";
            cmd = new SqliteCommand(sql, connection);
        }
        else
        {
            sql = "SELECT * FROM FoodLog WHERE Category = $category";
            cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$category", category);
        }

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string? cat = reader.IsDBNull(8) ? null : reader.GetString(8);
            string? notes = reader.IsDBNull(9) ? null : reader.GetString(9);

            foodList.Add(new FoodEntry(
                reader.GetInt32(0),
                reader.GetDateTime(1),
                reader.GetString(2),
                reader.GetDouble(3),
                reader.GetDouble(4),
                reader.GetDouble(5),
                reader.GetDouble(6),
                reader.GetDouble(7),
                cat,
                notes
            ));
        }

        if (foodList.Count == 0) return null;
        return foodList;
    }

    // Load selection
    public List<FoodEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        var FoodList = new List<FoodEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM FoodLog WHERE Time BETWEEN $startTime AND $endTime";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", startTime);
        cmd.Parameters.AddWithValue("$endTime", endTime);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string? category = reader.IsDBNull(8) ? null : reader.GetString(8);
            string notes = reader.IsDBNull(9) ? "" : reader.GetString(9);
            
            FoodList.Add(
                new FoodEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetString(2),
                    reader.GetDouble(3),
                    reader.GetDouble(4),
                    reader.GetDouble(5),
                    reader.GetDouble(6),
                    reader.GetDouble(7),
                    category,
                    notes
                )
            );
        }
        
        return FoodList;
    }
    
    // Period sum
    public double PeriodSum(string parameter, DateTime startTime, DateTime endTime)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = $"SELECT SUM({parameter}) FROM FoodLog WHERE Time BETWEEN $startTime AND $endTime";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", startTime);
        cmd.Parameters.AddWithValue("$endTime", endTime);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetDouble(0);
    }

    // Return list of parameter daily: 
    public List<(DateTime Day, double totalParameter)> DailySumRange(string parameter, DateTime startDate, DateTime endDate)
    {
        endDate.AddDays(1);
        var results = new List<(DateTime, double)>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql = @$"
        SELECT date(Time) AS Day, SUM({parameter}) AS TotalParameter
        FROM FoodLog
        WHERE Time >= $startDate AND Time < $endDate
        GROUP BY date(Time)
        ORDER BY Day;
        ";

        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startDate", startDate);
        cmd.Parameters.AddWithValue("$endDate", endDate);
        using var reader = cmd.ExecuteReader();

        while (reader.Read()) results.Add( (reader.GetDateTime(0), reader.GetDouble(1)) );

        return results;
    }


    // Update
    public void EditEntry(int id, FoodEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE FoodLog SET Time = $time, ItemName = $itemname, Amount = $amount, Calories = $calories, Protein = $protein, Carbs = $carbs, Fat = $fat, Category = $category, Notes = $notes WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$time", entry.Time);
        cmd.Parameters.AddWithValue("$itemname", entry.ItemName);
        cmd.Parameters.AddWithValue("$amount", entry.Amount);
        cmd.Parameters.AddWithValue("$calories", entry.Calories);
        cmd.Parameters.AddWithValue("$protein", entry.Protein);
        cmd.Parameters.AddWithValue("$carbs", entry.Carbs);
        cmd.Parameters.AddWithValue("$fat", entry.Fat);
        cmd.Parameters.AddWithValue("$category", entry.Category ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", entry.Notes ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }


    // Return last
    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM FoodLog ORDER BY Id DESC LIMIT 1";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetInt32(0);
    }

    // Delete
    public void DeleteEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "DELETE FROM FoodLog WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // Printing:
    private void Print(string text, [CallerMemberName] string caller = "")
    {
        RequestPrint?.Invoke(this, $"{caller}(): {text}");
    }
}