namespace MacroTrack.Core.Repositories;

using MacroTrack.Core.Infrastructure;
using MacroTrack.Core.Logging;
using MacroTrack.Core.Models;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

/// <summary>
/// Repository for interacting with Preset data.
/// </summary>
/// <remarks>
/// No logs.
/// </remarks>
public class PresetRepo : RepoBase
{
    private readonly string _connectionString;

    public PresetRepo(string connectionString, CoreContext ctx) : base(ctx)
    {
        _connectionString = connectionString;
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = @"
        CREATE TABLE IF NOT EXISTS Presets (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            PresetName TEXT NOT NULL,
            Calories DOUBLE NOT NULL,
            Protein DOUBLE NOT NULL,
            Carbs DOUBLE NOT NULL,
            Fat DOUBLE NOT NULL,
            Weight DOUBLE,
            Unit TEXT,
            Category TEXT,
            Notes TEXT
        );
        ";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    // new
    public void AddEntry(Preset entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "INSERT INTO Presets (PresetName, Calories, Protein, Carbs, Fat, Weight, Unit, Category, Notes) VALUES ($presetname, $calories, $protein, $carbs, $fat, $weight, $unit, $category, $notes)";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$presetname", entry.PresetName);
        cmd.Parameters.AddWithValue("$calories", entry.Calories);
        cmd.Parameters.AddWithValue("$protein", entry.Protein);
        cmd.Parameters.AddWithValue("$carbs", entry.Carbs);
        cmd.Parameters.AddWithValue("$fat", entry.Fat);
        cmd.Parameters.AddWithValue("$weight", entry.Weight ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$unit", entry.Unit ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$category", entry.Category ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", entry.Notes ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    // load
    public Preset? GetEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM Presets WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        
        double? weight = reader.IsDBNull(6) ? null : reader.GetDouble(6);
        string? unit = reader.IsDBNull(7) ? null : reader.GetString(7);
        string? category = reader.IsDBNull(8) ? null : reader.GetString(8);
        string? notes = reader.IsDBNull(9) ? null : reader.GetString(9);
        
        return new Preset(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5), weight, unit, category, notes);
    }

    // Load all
    public List<Preset> GetAll()
    {
        var presetList = new List<Preset>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT * FROM Presets";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            double? weight = reader.IsDBNull(6) ? null : reader.GetDouble(6);
            string? unit = reader.IsDBNull(7) ? null : reader.GetString(7);
            string? category = reader.IsDBNull(8) ? null : reader.GetString(8);
            string? notes = reader.IsDBNull(9) ? null : reader.GetString(9);
            
            presetList.Add(new Preset(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5), weight, unit, category, notes));
        }
        return presetList;
    }

    // Load all with a category
    public List<Preset> GetAllCategory(string? category)
    {
        var presetList = new List<Preset>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql;
        SqliteCommand  cmd;

        if (category is null)
        {
            sql = "SELECT * FROM Presets WHERE Category IS NULL";
            cmd = new SqliteCommand(sql, connection);
        }
        else
        {
            sql = "SELECT * FROM Presets WHERE Category = $category";
            cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$category", category);
        }

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            double? weight = reader.IsDBNull(6) ? null : reader.GetDouble(6);
            string? unit = reader.IsDBNull(7) ? null : reader.GetString(7);
            string? cat = reader.IsDBNull(8) ? null : reader.GetString(8);
            string? notes = reader.IsDBNull(9) ? null : reader.GetString(9);

            presetList.Add(new Preset(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5), weight, unit, cat, notes));
        }

        return presetList;
    }

    public List<string> GetCategoryList()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT DISTINCT Category FROM Presets";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        List<string> categories = new List<string>();
        while (reader.Read())
        {
            if (!reader.IsDBNull(0)) categories.Add(reader.GetString(0));
        }
        return categories;
    }

    // Load all names
    public List<string> GetAllNames()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT PresetName FROM Presets";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        List<string> names = new List<string>();
        while (reader.Read()) names.Add(reader.GetString(0));
        return names;
    }

    // Edit
    public void EditEntry(int id, Preset entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "UPDATE Presets SET PresetName = $presetname, Calories = $calories, Protein = $protein, Carbs = $carbs, Fat = $fat, Weight = $weight, Unit = $unit, Category = $category, Notes = $notes WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$presetname", entry.PresetName);
        cmd.Parameters.AddWithValue("$calories", entry.Calories);
        cmd.Parameters.AddWithValue("$protein", entry.Protein);
        cmd.Parameters.AddWithValue("$carbs", entry.Carbs);
        cmd.Parameters.AddWithValue("$fat", entry.Fat);
        cmd.Parameters.AddWithValue("$weight", entry.Weight ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$unit", entry.Unit ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$category", entry.Category ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$notes", entry.Notes ?? (object)DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    // return last
    public int ReturnLastId()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "SELECT Id FROM Presets ORDER BY Id DESC LIMIT 1";
        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return 0;
        return reader.GetInt32(0);
    }

    // delete
    public void DeleteEntry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = "DELETE FROM Presets WHERE Id = $id";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}