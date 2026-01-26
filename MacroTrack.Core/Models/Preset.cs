namespace MacroTrack.Core.Models;

public class Preset
{
    public int Id { get; set; }
    public string PresetName { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
    public double? Weight { get; set; }
    public string? Unit { get; set; }
    public string? Category { get; set; }
    public string? Notes { get; set; }

    // New with nullable optional parameters
    public Preset(string presetName, double calories, double protein, double carbs, double fat, double? weight, string? unit, string? category, string? notes)
    {
        PresetName = presetName;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;
        Weight = weight;
        Unit = unit;
        Category = category;
        Notes = notes;
    }

    // Load
    public Preset(int id, string presetName, double calories, double protein, double carbs, double fat, double? weight, string? unit, string? category, string? notes)
    {
        Id = id;
        PresetName = presetName;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;
        Weight = weight;
        Unit = unit;
        Category = category;
        Notes = notes;
    }
}