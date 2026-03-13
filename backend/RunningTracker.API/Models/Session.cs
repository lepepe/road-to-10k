namespace RunningTracker.API.Models;

public class Session
{
    public int Id { get; set; }
    public int Week { get; set; }          // 1–16
    public int SessionNumber { get; set; } // 1 or 2
    public bool Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
}
