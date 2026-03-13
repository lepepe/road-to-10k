namespace RunningTracker.API.Models;

public class TrainingWeek
{
    public int Id { get; set; }
    public int Week { get; set; }
    public int Phase { get; set; }
    public string Session1 { get; set; } = string.Empty;
    public string Session2 { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}
