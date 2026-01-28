namespace MacroTrack.Core.Repositories;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Models;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

/// <summary>
/// Repository for interacting with WeightLog data.
/// </summary>
/// <remarks>
/// No logs.
/// </remarks>
public class WeightLogRepo : RepoBase
{
    private readonly string _connectionString;

    public WeightLogRepo(string connectionString, CoreContext ctx) : base(ctx)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        CREATE TABLE IF NOT EXISTS WeightLog (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Time TEXT NOT NULL,
            Weight DOUBLE NOT NULL
        );
        ";

        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    // New
    public void AddEntry(WeightEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "INSERT INTO WeightLog (Time, Weight) VALUES ($time, $weight)";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$time", entry.Time);
        cmd.Parameters.AddWithValue("$weight", entry.Weight);
        cmd.ExecuteNonQuery();
    }

    // Load
    public WeightEntry? GetEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM WeightLog WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read()) return null;
        
        return new WeightEntry(
            reader.GetInt32(0),
            reader.GetDateTime(1),
            reader.GetDouble(2)
        );
    } 

    // Load all
    public List<WeightEntry> GetAll()
    {
        var WeightList = new List<WeightEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM WeightLog";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            WeightList.Add(
                new WeightEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetDouble(2)
                )
            );
        }
        return WeightList;
    }

    // All data between two dates
    public List<WeightEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        var WeightList = new List<WeightEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM WeightLog WHERE Time BETWEEN $startTime AND $endTime";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", startTime);
        cmd.Parameters.AddWithValue("$endTime", endTime);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            WeightList.Add(
                new WeightEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetDouble(2)
                )
            );
        }
        return WeightList;
    }

    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM WeightLog ORDER BY Id DESC LIMIT 1";
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
        string sql = "DELETE FROM WeightLog WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}