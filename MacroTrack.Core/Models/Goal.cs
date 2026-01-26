namespace MacroTrack.Core.Models;

public class Goal
{
    public int Id { get; set; }
    public string GoalName { get; set; }

    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }

    public string? GoalType { get; set; }
    public string? Notes { get; set; }

    public double? MinCal { get; set; }
    public double? MaxCal { get; set; }
    public double? MinPro { get; set; }
    public double? MaxPro { get; set; }
    public double? MinCar { get; set; }
    public double? MaxCar { get; set; }
    public double? MinFat { get; set; }
    public double? MaxFat { get; set; }

    // New
    public Goal(string goalName, double calories, double protein, double carbs, double fat, string? notes, string? goalType, double? minCal, double? maxCal, double? minPro, double? maxPro, double? minCar, double? maxCar, double? minFat, double? maxFat)
    {
        GoalName = goalName;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;

        Notes = notes;
        GoalType = goalType;

        MinCal = minCal;
        MaxCal = maxCal;
        MinPro = minPro;
        MaxPro = maxPro;
        MinCar = minCar;
        MaxCar = maxCar;
        MinFat = minFat;
        MaxFat = maxFat;
    }

    // Load
    public Goal(int id, string goalName, double calories, double protein, double carbs, double fat, string? notes, string? goalType, double? minCal, double? maxCal, double? minPro, double? maxPro, double? minCar, double? maxCar, double? minFat, double? maxFat)
    {
        Id = id;
        GoalName = goalName;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;

        Notes = notes;
        GoalType = goalType;

        MinCal = minCal;
        MaxCal = maxCal;
        MinPro = minPro;
        MaxPro = maxPro;
        MinCar = minCar;
        MaxCar = maxCar;
        MinFat = minFat;
        MaxFat = maxFat;
    }
}