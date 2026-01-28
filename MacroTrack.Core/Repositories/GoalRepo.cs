// GoalRepo will be both Goal Registry and Goal History.

namespace MacroTrack.Core.Repositories;

using MacroTrack.Core.AppModels;
using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Repository for interacting with Goal data, being GoalRegistry and GoalHistory.
/// </summary>
/// <remarks>
/// No logs.
/// </remarks>
public class GoalRepo : RepoBase
{
    private readonly string _connectionString;

    public GoalRepo(string connectionString, CoreContext ctx) : base(ctx)
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
        CREATE TABLE IF NOT EXISTS GoalRegistry (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Calories DOUBLE NOT NULL,
            Protein DOUBLE NOT NULL,
            Carbs DOUBLE NOT NULL,
            Fat DOUBLE NOT NULL,
            Type TEXT,
            Notes TEXT,
            MinCal DOUBLE,
            MaxCal DOUBLE,
            MinPro DOUBLE,
            MaxPro DOUBLE,
            MinCar DOUBLE,
            MaxCar DOUBLE,
            MinFat DOUBLE,
            MaxFat DOUBLE
        );
        ";
        using var cmd1 = new SqliteCommand(sql1, connection);
        cmd1.ExecuteNonQuery();

        string sql2 = @"
        CREATE TABLE IF NOT EXISTS GoalHistory (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            GoalId INTEGER NOT NULL,
            ActivatedAt TEXT NOT NULL,
            DeactivatedAt TEXT,
            FOREIGN KEY (GoalId) REFERENCES GoalRegistry(Id)
        );
        ";
        using var cmd2 = new SqliteCommand(sql2, connection);
        cmd2.ExecuteNonQuery();
    }

    // New Goal
    public void AddGoal(Goal goal)
    {
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        INSERT INTO GoalRegistry (Name, Calories, Protein, Carbs, Fat, Type, Notes, MinCal, MaxCal, MinPro, MaxPro, MinCar, MaxCar, MinFat, MaxFat)
        VALUES ($name, $calories, $protein, $carbs, $fat, $type, $notes, $mincal, $maxcal, $minpro, $maxpro, $mincar, $maxcar, $minfat, $maxfat);
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$name", goal.GoalName);
        cmd.Parameters.AddWithValue("$calories", goal.Calories);
        cmd.Parameters.AddWithValue("$protein", goal.Protein);
        cmd.Parameters.AddWithValue("$carbs", goal.Carbs);
        cmd.Parameters.AddWithValue("$fat", goal.Fat);
        cmd.Parameters.AddWithValue("$type", goal.GoalType ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", goal.Notes ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$mincal", goal.MinCal.HasValue ? (object)goal.MinCal.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxcal", goal.MaxCal.HasValue ? (object)goal.MaxCal.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$minpro", goal.MinPro.HasValue ? (object)goal.MinPro.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxpro", goal.MaxPro.HasValue ? (object)goal.MaxPro.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$mincar", goal.MinCar.HasValue ? (object)goal.MinCar.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxcar", goal.MaxCar.HasValue ? (object)goal.MaxCar.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$minfat", goal.MinFat.HasValue ? (object)goal.MinFat.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxfat", goal.MaxFat.HasValue ? (object)goal.MaxFat.Value : DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    // Load Goal
    public Goal? GetGoal(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM GoalRegistry WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new Goal(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetDouble(2),
            reader.GetDouble(3),
            reader.GetDouble(4),
            reader.GetDouble(5),
            reader.IsDBNull(6) ? null : reader.GetString(6),
            reader.IsDBNull(7) ? null : reader.GetString(7),
            reader.IsDBNull(8) ? null : reader.GetDouble(8),
            reader.IsDBNull(9) ? null : reader.GetDouble(9),
            reader.IsDBNull(10) ? null : reader.GetDouble(10),
            reader.IsDBNull(11) ? null : reader.GetDouble(11),
            reader.IsDBNull(12) ? null : reader.GetDouble(12),
            reader.IsDBNull(13) ? null : reader.GetDouble(13),
            reader.IsDBNull(14) ? null : reader.GetDouble(14),
            reader.IsDBNull(15) ? null : reader.GetDouble(15)
        );
    }

    // Just return calories (used for constructing goal time series):
    public double GetCalories(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Calories FROM GoalRegistry WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return -1;
        return reader.GetDouble(0);
    }

    // Load all Goals
    public List<Goal> GetAllGoals()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM GoalRegistry";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        var goals = new List<Goal>();
        while (reader.Read())
        {
            goals.Add(new Goal(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetDouble(2),
                reader.GetDouble(3),
                reader.GetDouble(4),
                reader.GetDouble(5),
                reader.IsDBNull(6) ? null : reader.GetString(6),
                reader.IsDBNull(7) ? null : reader.GetString(7),
                reader.IsDBNull(8) ? null : reader.GetDouble(8),
                reader.IsDBNull(9) ? null : reader.GetDouble(9),
                reader.IsDBNull(10) ? null : reader.GetDouble(10),
                reader.IsDBNull(11) ? null : reader.GetDouble(11),
                reader.IsDBNull(12) ? null : reader.GetDouble(12),
                reader.IsDBNull(13) ? null : reader.GetDouble(13),
                reader.IsDBNull(14) ? null : reader.GetDouble(14),
                reader.IsDBNull(15) ? null : reader.GetDouble(15)
            ));
        }
        return goals;
    }

    // Edit Goal
    public void EditGoal(int id, Goal goal)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE GoalRegistry SET Name = $name, Calories = $calories, Protein = $protein, Carbs = $carbs, Fat = $fat, Type = $type, Notes = $notes, MinCal = $mincal, MaxCal = $maxcal, MinPro = $minpro, MaxPro = $maxpro, MinCar = $mincar, MaxCar = $maxcar, MinFat = $minfat, MaxFat = $maxfat WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$name", goal.GoalName);
        cmd.Parameters.AddWithValue("$calories", goal.Calories);
        cmd.Parameters.AddWithValue("$protein", goal.Protein);
        cmd.Parameters.AddWithValue("$carbs", goal.Carbs);
        cmd.Parameters.AddWithValue("$fat", goal.Fat);
        cmd.Parameters.AddWithValue("$type", goal.GoalType ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", goal.Notes ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$mincal", goal.MinCal.HasValue ? (object)goal.MinCal.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxcal", goal.MaxCal.HasValue ? (object)goal.MaxCal.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$minpro", goal.MinPro.HasValue ? (object)goal.MinPro.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxpro", goal.MaxPro.HasValue ? (object)goal.MaxPro.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$mincar", goal.MinCar.HasValue ? (object)goal.MinCar.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxcar", goal.MaxCar.HasValue ? (object)goal.MaxCar.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$minfat", goal.MinFat.HasValue ? (object)goal.MinFat.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$maxfat", goal.MaxFat.HasValue ? (object)goal.MaxFat.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // Return last
    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM GoalRegistry ORDER BY Id DESC LIMIT 1";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetInt32(0);
    }

    // Delete Goal
    public void DeleteGoal(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "DELETE FROM GoalRegistry WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    // Activate Goal
    public void ActivateGoal(GoalActivation ga)
    {
        // Creation of the GoalActivation is done by service, who should also be able to decide on time so you can set them in the past and future if you would like. 
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "INSERT INTO GoalHistory (GoalId, ActivatedAt) values ($GoalId, $ActivatedAt)";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$GoalId", ga.GoalId);
        cmd.Parameters.AddWithValue("$ActivatedAt", ga.ActivatedAt.ToString("yyyy-MM-dd"));
        cmd.ExecuteNonQuery();
    }

    // Deactivate (have no goals currently)
    public void Deactivate(int id, DateOnly deactivatedAt)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE GoalHistory SET DeactivatedAt = $DeactivatedAt WHERE Id = $Id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$DeactivatedAt", deactivatedAt.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$Id", id);
        cmd.ExecuteNonQuery();
    }
    
    

    // Deactivate Goal (delete goal activation: We never had this goal!)
    public void DeleteGoalActivation(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "DELETE FROM GoalHistory WHERE Id = $Id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$Id", id);
        cmd.ExecuteNonQuery();
    }

    // get goalactivation by ID
    public GoalActivation? GetGoalActivation(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM GoalHistory WHERE Id = $Id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$Id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        int Id = reader.GetInt32(0);
        int GoalId = reader.GetInt32(1);
        DateOnly ActivatedAt = DateOnly.Parse(reader.GetString(2));
        if (reader.IsDBNull(3))
        {
            return new GoalActivation(Id, GoalId, ActivatedAt);
        }
        else
        {
            return new GoalActivation(Id, GoalId, ActivatedAt, DateOnly.Parse(reader.GetString(3)));
        }
    }

    // Return last Activation
    public int ReturnLastActivationId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM GoalHistory ORDER BY Id DESC LIMIT 1";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetInt32(0);
    }

    // Get present goal
    public GoalActivation? GetPresentGoal(DateOnly presentDate)
    {
        // "Present date" is given as an argument instead of going for "DateOnly.FromDateTime(DateTime.Now)", because we might like to use this to see what was going on at other times, could be useful, idk. It isn't a nullable variable because that doesn't sound very REPO-ey, that's something for service to do imo.
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
            SELECT * FROM GoalHistory 
            WHERE ActivatedAt <= $presentDate 
            AND (DeactivatedAt IS NULL OR DeactivatedAt > $presentDate)
            ORDER BY ActivatedAt DESC 
            LIMIT 1
        ";
        // theoretically we wouldn't need those last 2 lines, but we'll keep them just to be sure.
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$presentDate", presentDate.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        int Id = reader.GetInt32(0);
        int GoalId = reader.GetInt32(1);
        DateOnly ActivatedAt = DateOnly.Parse(reader.GetString(2));
        if (reader.IsDBNull(3))
        {
            return new GoalActivation(Id, GoalId, ActivatedAt);
        }
        else
        {
            return new GoalActivation(Id, GoalId, ActivatedAt, DateOnly.Parse(reader.GetString(3)));
        }
        // alternatively this could just return GetGoal(Id) for actual goal, but we will do it this way so that we can also get information about how long the goal has been in place. 
    }

    // Find next goal from date (useful for setting deactivation date)
    public GoalActivation? GetNextGoal(DateOnly presentDate)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
            SELECT * FROM GoalHistory 
            WHERE ActivatedAt > $presentDate 
            ORDER BY ActivatedAt ASC
            LIMIT 1
        "; // ActivatedAt > $presentDate, not >=, because >= will return activations from the present date, which there necessarily will be if we're making a new one today: instead of returning null due to there being none AFTER this new one.
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$presentDate", presentDate.ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        int Id = reader.GetInt32(0);
        int GoalId = reader.GetInt32(1);
        DateOnly ActivatedAt = DateOnly.Parse(reader.GetString(2));
        if (reader.IsDBNull(3))
        {
            return new GoalActivation(Id, GoalId, ActivatedAt);
        }
        else
        {
            return new GoalActivation(Id, GoalId, ActivatedAt, DateOnly.Parse(reader.GetString(3)));
        }
    }

    public List<GoalActivation> GetGoalHistoryInterval(DateTime startTime, DateTime endTime)
    {
        var GoalHistory = new List<GoalActivation>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        // Get goals that were active at any point during the time period
        // A goal is active during the period if: ActivatedAt <= endTime AND (DeactivatedAt IS NULL OR DeactivatedAt >= startTime)
        string sql = "SELECT * FROM GoalHistory WHERE ActivatedAt <= $endTime AND (DeactivatedAt IS NULL OR DeactivatedAt >= $startTime) ORDER BY ActivatedAt";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$startTime", DateOnly.FromDateTime(startTime).ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$endTime", DateOnly.FromDateTime(endTime).ToString("yyyy-MM-dd"));
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (reader.IsDBNull(3))
            {
                GoalHistory.Add(
                    new GoalActivation(
                        reader.GetInt32(0), 
                        reader.GetInt32(1), 
                        DateOnly.Parse(reader.GetString(2))
                    )
                );
            }
            else
            {
                GoalHistory.Add(
                    new GoalActivation(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        DateOnly.Parse(reader.GetString(2)),
                        DateOnly.Parse(reader.GetString(3))
                    )
                );
            }
        }
        return GoalHistory;
    }


    // Get goal history
    public List<GoalActivation> GetGoalHistory()
    {
        var GoalHistory = new List<GoalActivation>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM GoalHistory ORDER BY ActivatedAt";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (reader.IsDBNull(3))
            {
                GoalHistory.Add(
                    new GoalActivation(
                        reader.GetInt32(0), 
                        reader.GetInt32(1), 
                        DateOnly.Parse(reader.GetString(2))
                    )
                );
            }
            else
            {
                GoalHistory.Add(
                    new GoalActivation(
                        reader.GetInt32(0), 
                        reader.GetInt32(1), 
                        DateOnly.Parse(reader.GetString(2)),
                        DateOnly.Parse(reader.GetString(3))
                    )
                );
            }
        }
        return GoalHistory;

    }

    // Get goal parameter


    /* // Get goal parameter between dates
    public List<double> GetParamaterHistory(string parameter, DateTime startTime, DateTime endTime)
    {
        // There isn't really an easy way to get a datatable out here, we would need to make "record" classes, which is annoying and I really don't want to do that right now. If and when you want to do it you can do it here but not now. 
    } */

}