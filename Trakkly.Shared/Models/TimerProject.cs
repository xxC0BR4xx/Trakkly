namespace Trakkly.Shared.Models;

public enum PomodoroPhase
{
    Work,
    Break
}

public class TimerProject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRunning { get; set; }

    // Pomodoro settings
    public bool UsePomodoroMode { get; set; }
    public int WorkDurationMinutes { get; set; } = 25;
    public int BreakDurationMinutes { get; set; } = 5;
    public PomodoroPhase CurrentPhase { get; set; } = PomodoroPhase.Work;
    public DateTime? PomodoroPhaseStartTime { get; set; }

    public List<TimerEntry> Entries { get; set; } = new();
    private TimerEntry? _currentEntry;

    public TimerProject()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Description = string.Empty;
        IsRunning = false;
        UsePomodoroMode = false;
    }

    public void StartTimer()
    {
        if (IsRunning) return;

        IsRunning = true;
        if (UsePomodoroMode)
        {
            PomodoroPhaseStartTime = DateTime.Now;
        }

        _currentEntry = new TimerEntry
        {
            StartTime = DateTime.Now,
            IsBreak = false
        };
        Entries.Add(_currentEntry);
    }

    public void StopTimer()
    {
        if (!IsRunning || _currentEntry == null) return;

        var now = DateTime.Now;

        _currentEntry.EndTime = now;
        _currentEntry.RecordedDuration = now - _currentEntry.StartTime;

        IsRunning = false;
        _currentEntry = null;
        PomodoroPhaseStartTime = null;
    }

    public TimeSpan GetPomodoroTimeRemaining()
    {
        if (!UsePomodoroMode || !IsRunning || !PomodoroPhaseStartTime.HasValue)
            return TimeSpan.Zero;

        var elapsed = DateTime.Now - PomodoroPhaseStartTime.Value;
        var totalDuration = CurrentPhase == PomodoroPhase.Work 
            ? TimeSpan.FromMinutes(WorkDurationMinutes)
            : TimeSpan.FromMinutes(BreakDurationMinutes);

        var remaining = totalDuration - elapsed;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public void AdvancePomodoroPhase()
    {
        if (!UsePomodoroMode) return;

        CurrentPhase = CurrentPhase == PomodoroPhase.Work ? PomodoroPhase.Break : PomodoroPhase.Work;
        PomodoroPhaseStartTime = DateTime.Now;

        if (CurrentPhase == PomodoroPhase.Break && _currentEntry != null)
        {
            _currentEntry.EndTime = DateTime.Now;
            _currentEntry.RecordedDuration = _currentEntry.EndTime - _currentEntry.StartTime;

            _currentEntry = new TimerEntry
            {
                StartTime = DateTime.Now,
                IsBreak = true
            };
            Entries.Add(_currentEntry);
        }
        else if (CurrentPhase == PomodoroPhase.Work && _currentEntry != null)
        {
            _currentEntry.EndTime = DateTime.Now;
            _currentEntry.RecordedDuration = _currentEntry.EndTime - _currentEntry.StartTime;

            _currentEntry = new TimerEntry
            {
                StartTime = DateTime.Now,
                IsBreak = false
            };
            Entries.Add(_currentEntry);
        }
    }


    public void ResetTimer()
    {
        IsRunning = false;
        _currentEntry = null;
        Entries.Clear();
    }

    public TimeSpan Elapsed
    {
        get
        {
            var total = Entries
                .Where(e => e.EndTime.HasValue && !e.IsBreak)
                .Aggregate(TimeSpan.Zero, (sum, e) => sum + e.Duration);

            if (IsRunning && _currentEntry?.EndTime == null && !_currentEntry?.IsBreak == true)
            {
                total += DateTime.Now - _currentEntry.StartTime;
            }

            return total;
        }
    }
    
    public TimeSpan GetElapsedToday()
    {
        var today = DateTime.Today;
        return Entries
            .Where(e => e.StartTime.Date == today && !e.IsBreak)
            .Aggregate(TimeSpan.Zero, (total, e) => total + e.Duration);
    }

    public TimeSpan GetElapsedThisWeek()
    {
        var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        return Entries
            .Where(e => e.StartTime >= startOfWeek && !e.IsBreak)
            .Aggregate(TimeSpan.Zero, (total, e) => total + e.Duration);
    }

    public TimeSpan GetElapsedThisMonth()
    {
        var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        return Entries
            .Where(e => e.StartTime >= startOfMonth && !e.IsBreak)
            .Aggregate(TimeSpan.Zero, (total, entry) => total + entry.Duration);
    }

    
}
    public class TimerEntry
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? RecordedDuration { get; set; }
        public bool IsBreak { get; set; }

        public TimeSpan Duration => 
            (EndTime.HasValue ? EndTime.Value : DateTime.Now) - StartTime;
    }

