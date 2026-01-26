namespace MacroTrack.Core.Models;

public class FoodEntry
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string ItemName { get; set; }
    public double Amount { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
    public string? Category { get; set; }
    public string? Notes { get; set; }

    // New
    public FoodEntry(DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        Time = time;
        ItemName = itemName;
        Amount = amount;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;
        Category = category;
        Notes = notes;
    }

    // Load
    public FoodEntry(int id, DateTime time, string itemName, double amount, double calories, double protein, double carbs, double fat, string? category, string? notes)
    {
        Id = id;
        Time = time;
        ItemName = itemName;
        Amount = amount;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;
        Category = category;
        Notes = notes;
    }
}