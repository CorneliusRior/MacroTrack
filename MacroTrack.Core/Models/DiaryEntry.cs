namespace MacroTrack.Core.Models;

public class DiaryEntry
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string Body { get; set; }

    public DiaryEntry(DateTime time, string body) // for making new entries
    {
        Time = time;
        Body = body;
    }

    public DiaryEntry(int id, DateTime time, string body) // for loading and editing entries.
    {
        Id = id;
        Time = time;
        Body = body;
    }
}