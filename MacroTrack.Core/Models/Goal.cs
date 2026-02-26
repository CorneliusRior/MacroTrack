namespace MacroTrack.Core.Models;

public class Goal
{
    public int Id { get; set; }
    public string GoalName { get; set; }

    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }

    public GoalType GoalType { get; set; }
    //public string? GoalType { get; set; }
    public string? CustomType { get; set; }
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
    public Goal(string goalName, double calories, double protein, double carbs, double fat, GoalType goalType = GoalType.None, string? customType = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar =  null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        GoalName = goalName;    // 0
        Calories = calories;    // 1
        Protein = protein;      // 2
        Carbs = carbs;          // 3
        Fat = fat;              // 4

        GoalType = goalType;    // 5
        CustomType = GoalType == GoalType.Custom ? customType : null;
        Notes = notes;          // 7

        MinCal = minCal;        // 8
        MaxCal = maxCal;        // 9
        MinPro = minPro;        // 10
        MaxPro = maxPro;        // 11
        MinCar = minCar;        // 12
        MaxCar = maxCar;        // 13
        MinFat = minFat;        // 14
        MaxFat = maxFat;        // 15
    }

    // Load
    public Goal(int id, string goalName, double calories, double protein, double carbs, double fat, GoalType goalType = GoalType.None, string? customType = null, string? notes = null, double? minCal = null, double? maxCal = null, double? minPro = null, double? maxPro = null, double? minCar = null, double? maxCar = null, double? minFat = null, double? maxFat = null)
    {
        Id = id;
        GoalName = goalName;
        Calories = calories;
        Protein = protein;
        Carbs = carbs;
        Fat = fat;

        GoalType = goalType;
        CustomType = customType;
        Notes = notes;

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

