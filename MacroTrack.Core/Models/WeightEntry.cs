namespace MacroTrack.Core.Models;

public class WeightEntry
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public double Weight { get; set; }

    // Constructor for new entries
    public WeightEntry(DateTime time, double weight)
    {
        Time = time;
        Weight = weight;
    }

    // Constructor for loading entries:
    public WeightEntry(int id, DateTime time, double weight)
    {
        Id = id;
        Time = time;
        Weight = weight;
    }
}