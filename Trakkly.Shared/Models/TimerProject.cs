namespace Trakkly.Shared.Models;

public class TimerProject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRunning { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public TimeSpan AccumulatedTime { get; set; } = TimeSpan.Zero;


    public TimerProject()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Description = string.Empty;
        IsRunning = false;
    }

    public void StartTimer()
    {
        IsRunning = true;
        StartTime = DateTime.Now;
    }

    public void StopTimer()
    {
        IsRunning = false;
        AccumulatedTime += DateTime.Now - StartTime;

    }
    
    public void ResetTimer()
    {
        IsRunning = false;
        AccumulatedTime = TimeSpan.Zero;
        StartTime = DateTime.MinValue;
    }

    public TimeSpan Elapsed =>
        IsRunning ? AccumulatedTime + (DateTime.Now - StartTime) : AccumulatedTime;
}