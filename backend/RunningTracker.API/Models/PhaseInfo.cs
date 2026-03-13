namespace RunningTracker.API.Models;

public class PhaseInfo
{
    public int Phase { get; set; }   // PK (1, 2, 3)
    public string DotColor { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
