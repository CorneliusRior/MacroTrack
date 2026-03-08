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

    // Print:
    const int PrintIdSpace = -6;
    const int PrintNameSpace = -20;
    const int PrintCalSpace = 7;
    const int PrintMacroSpace = 6;
    const int PrintWeightSpace = 8;
    const int PrintCategorySpace = -15;
    const int PrintNotesSpace = -100;
    public static string PrintHeader() => $"{"ID:", PrintIdSpace}{"Preset Name:", PrintNameSpace}{"Cal.:", PrintCalSpace}{"Pro.:", PrintMacroSpace}{"Car.:",PrintMacroSpace}{"Fat:",PrintMacroSpace}{"Weight:", PrintWeightSpace}  {"Category:", PrintCategorySpace}{"Notes:", PrintNotesSpace}";
    public string Print() => $"{$"#{Id}", PrintIdSpace}{PresetName.Truncate(PrintNameSpace), PrintNameSpace}{Calories.ToStringTruncate(PrintCalSpace), PrintCalSpace}{Protein.ToStringTruncate(PrintMacroSpace), PrintMacroSpace}{Carbs.ToStringTruncate(PrintMacroSpace),PrintMacroSpace}{Fat, PrintMacroSpace}{(Weight is null ? "-" : Weight.Value.ToStringTruncate(PrintWeightSpace, suffix: Unit ?? string.Empty)), PrintWeightSpace}  {Category.TruncateNullable(PrintCategorySpace) ?? "-", PrintCategorySpace}{Notes.TruncateNullable(PrintNotesSpace) ?? "-", PrintNotesSpace}";
}