namespace RunningTracker.API.Models;

public class ScheduleDay
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Day { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool IsRun { get; set; }
}
