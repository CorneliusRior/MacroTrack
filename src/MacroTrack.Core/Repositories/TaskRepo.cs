using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MacroTrack.Core.Repositories;

/// <summary>
/// Repository for interacting with Task data, including TaskRegistry, TaskLog, and TaskCompletion.
/// </summary>
/// <remarks>
/// No logs.
/// </remarks>
public class TaskRepo : RepoBase
{
    private readonly string _connectionString;

    public TaskRepo(string connectionString, CoreContext ctx) : base(ctx)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }


    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var cmd = new SqliteCommand("PRAGMA foreign_keys = ON;", connection);
        cmd.ExecuteNonQuery();
        string sql1 = @"
        CREATE TABLE IF NOT EXISTS TaskRegistry (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Description TEXT,
            IsActive INTEGER NOT NULL CHECK (IsActive in (0,1))
        );
        ";
        using var cmd1 = new SqliteCommand(sql1, connection);
        cmd1.ExecuteNonQuery();

        string sql2 = @"
        CREATE TABLE IF NOT EXISTS TaskLog (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            LogDate TEXT NOT NULL UNIQUE,
            Cheat INTEGER NOT NULL CHECK (Cheat IN (0,1))
        );
        ";
        using var cmd2 = new SqliteCommand(sql2, connection);
        cmd2.ExecuteNonQuery();

        string sql3 = @"
        CREATE TABLE IF NOT EXISTS TaskCompletion (
            TaskId INTEGER NOT NULL,
            LogDate TEXT NOT NULL,
            Completed INTEGER NOT NULL CHECK (Completed IN (0,1)),
            PRIMARY KEY (TaskId, LogDate),
            FOREIGN KEY (TaskId) REFERENCES TaskRegistry(Id)
        );
        ";
        using var cmd3 = new SqliteCommand(sql3, connection);
        cmd3.ExecuteNonQuery();
    }

    // Cheat Day:
    public void SetCheatDay(DateTime date, bool isCheatDay)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
            INSERT INTO TaskLog (LogDate, Cheat) 
            VALUES ($logDate, $cheat)
            ON CONFLICT(LogDate)
            DO UPDATE SET Cheat = excluded.Cheat
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$cheat", isCheatDay? 1 : 0);
        cmd.ExecuteNonQuery();
    }

    public bool GetIsCheatDay(DateTime date)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Cheat FROM TaskLog WHERE LogDate = $logDate";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return false;
        return reader.GetInt32(0) == 1 ? true : false;
    }

    public List<DateTime> GetCheatDayRange(DateTime startTime, DateTime endTime)
    {
        List<DateTime> cheatList = new();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT LogDate FROM TaskLog WHERE LogDate >= $startTime AND LogDate <= $endTime AND Cheat = 1 ORDER BY LogDate";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", startTime.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$endTime", endTime.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) cheatList.Add(DateTime.Parse(reader.GetString(0)));
        return cheatList;
    }

    // New
    public void AddTask(DailyTask entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "INSERT INTO TaskRegistry (Name, Description, IsActive) VALUES ($name, $description, $isActive)";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$name", entry.Name);
        cmd.Parameters.AddWithValue("$description", entry.Description ?? "");
        cmd.Parameters.AddWithValue("$isActive", entry.IsActive ? 1 : 0);
        cmd.ExecuteNonQuery();
    }
    
    // Load one w/ completed:
    public DailyTask? GetTask(int id, DateTime date)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        SELECT r.Id, r.Name, r.Description, r.IsActive,
            COALESCE(c.Completed, 0) as Completed
        FROM TaskRegistry r
        LEFT JOIN TaskCompletion c
            ON c.TaskId = r.Id AND c.LogDate = $logDate
        WHERE Id = $id;
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue($"logDate", date.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        string description = reader.IsDBNull(2) ? "" : reader.GetString(2);
        bool isActive = reader.GetInt32(3) == 1;
        bool completed = reader.GetInt32(4) == 1;

        return new DailyTask(reader.GetInt32(0), reader.GetString(1), description, isActive, completed);
    }

    // Load all w/ completed
    public List<DailyTask> GetAll(DateTime date)
    {
        var TaskList = new List<DailyTask>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        SELECT r.Id, r.Name, r.Description, r.IsActive,
            COALESCE(c.Completed, 0) AS Completed
        FROM TaskRegistry r
        LEFT JOIN TaskCompletion c
            ON c.TaskId = r.Id AND c.LogDate = $logDate
        ORDER BY r.Id;
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string description = reader.IsDBNull(2) ? "" : reader.GetString(2);
            bool isActive = reader.GetInt32(3) == 1;
            bool completed = reader.GetInt32(4) == 1;

            TaskList.Add(
                new DailyTask(reader.GetInt32(0), reader.GetString(1), description, isActive, completed)
            );
        }
        return TaskList;
    }

    // Load all w/ completed & streaks
    public List<DailyTask> GetAllStreaks(DateTime date)
    {
        var TaskList = new List<DailyTask>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql = @"
            WITH done AS (
                SELECT TaskId, LogDate AS d
                FROM TaskCompletion
                WHERE Completed = 1
                    AND LogDate <= $logDate
            ),
            grp AS (
                SELECT TaskId, d,
                    julianday(d) - ROW_NUMBER() OVER (PARTITION BY TaskId ORDER BY d) as g
                FROM done
            ),
            current_grp AS (
                SELECT TaskId, g
                FROM grp
                WHERE d = $logDate
            ),
            streaks AS (
                SELECT gr.TaskId, Count(*) AS Streak
                FROM grp gr
                JOIN current_grp cg
                    ON cg.TaskId = gr.TaskId AND cg.g = gr.g
                GROUP BY gr.TaskId
            )
            SELECT r.Id, r.Name, r.Description, r.IsActive,
                COALESCE(c.Completed, 0) AS Completed,
                COALESCE(s.Streak, 0) AS Streak
            FROM TaskRegistry r
            LEFT JOIN TaskCompletion c
                ON c.TaskId = r.Id AND c.LogDate = $logDate
            LEFT JOIN streaks s
                ON s.TaskId = r.Id
            ORDER BY r.Id;
            ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string description = reader.IsDBNull(2) ? "" : reader.GetString(2);
            bool isActive = reader.GetInt32(3) == 1;
            bool completed = reader.GetInt32(4) == 1;

            TaskList.Add(
                new DailyTask(reader.GetInt32(0), reader.GetString(1), description, isActive, completed, reader.GetInt32(5))
            );
        }
        return TaskList;
    }

    // Load Registry List
    public List<DailyTask> GetRegistry()
    {
        var TaskList = new List<DailyTask>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM TaskRegistry";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string description = reader.IsDBNull(2) ? "" : reader.GetString(2);
            bool isActive = reader.GetInt32(3) == 1;
            
            TaskList.Add(
                new DailyTask(reader.GetInt32(0), reader.GetString(1), description, isActive)
            );
        }
        return TaskList;
    }

    // Deactivate task
    public void DeactivateEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE TaskRegistry SET IsActive = $isActive WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$isActive", 0);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // Activate task
    public void ActivateEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE TaskRegistry SET IsActive = $isActive WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$isActive", 1);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // return last id
    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM TaskRegistry ORDER BY Id DESC LIMIT 1";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetInt32(0);
    }

    public void SetComplete(int id, DateTime date)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        INSERT INTO TaskCompletion (TaskId, LogDate, Completed) 
        VALUES ($taskId, $logDate, $completed)
        ON CONFLICT(TaskId, LogDate)
        DO UPDATE SET Completed = excluded.Completed;
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$taskId", id);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$completed", 1);
        cmd.ExecuteNonQuery();
    }

    public void SetIncomplete(int id, DateTime date)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        INSERT INTO TaskCompletion (TaskId, LogDate, Completed) 
        VALUES ($taskId, $logDate, $completed)
        ON CONFLICT(TaskId, LogDate)
        DO UPDATE SET Completed = excluded.Completed;
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$taskId", id);
        cmd.Parameters.AddWithValue("$logDate", date.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$completed", 0);
        cmd.ExecuteNonQuery();
    }

}