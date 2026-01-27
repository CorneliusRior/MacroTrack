namespace MacroTrack.Core.Repositories;

using Microsoft.Data.Sqlite;
using MacroTrack.Core.Models;

using System.Runtime.CompilerServices;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Infrastructure;

public class DiaryRepo : RepoBase
{
    private readonly string _connectionString;

    public DiaryRepo(string connectionString, CoreContext ctx) : base(ctx)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }

    public void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        CREATE TABLE IF NOT EXISTS Diary (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Time TEXT NOT NULL,
            Body TEXT NOT NULL
        );
        ";

        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    // new
    public void AddEntry(DiaryEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "INSERT INTO Diary (Time, Body) VALUES ($time, $body)";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$time", entry.Time);
        cmd.Parameters.AddWithValue("$body", entry.Body);
        cmd.ExecuteNonQuery();
    }

    // load
    public DiaryEntry? GetEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM Diary WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        
        if (!reader.Read()) return null;

        return new DiaryEntry(
            reader.GetInt32(0),
            reader.GetDateTime(1),
            reader.GetString(2)
        );
    }

    // Load all
    public List<DiaryEntry> GetAll()
    {
        var DiaryList = new List<DiaryEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM Diary";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            DiaryList.Add(
                new DiaryEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetString(2)
                )
            );
        }
        return DiaryList;
    }

    // Load all between dates
    public List<DiaryEntry> FromTimes(DateTime startTime, DateTime endTime)
    {
        var DiaryList = new List<DiaryEntry>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM Diary WHERE Time BETWEEN $startTime AND $endTime";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", startTime);
        cmd.Parameters.AddWithValue("$endTime", endTime);
        using var reader = cmd.ExecuteReader();        
        while (reader.Read())
        {
            DiaryList.Add(
                new DiaryEntry(
                    reader.GetInt32(0),
                    reader.GetDateTime(1),
                    reader.GetString(2)
                )
            );
        }
        return DiaryList;
    }

    // Edit
    public void EditEntry(int id, string body) // was considering having this as DiaryEntry Entry.
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE Diary SET Body = $body WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$body", body);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // return last
    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM Diary ORDER BY Id DESC LIMIT 1";
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
        string sql = "DELETE FROM Diary WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}