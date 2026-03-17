using System.Text.Json;
using Trakkly.Shared.Models;

public class ProjectManager
{
    private readonly IProjectStorage _storage;
    public List<TimerProject> Projects { get; private set; } = new();
    public bool IsInitialized { get; private set; } = false;
    public event Action? OnProjectsChanged;

    public ProjectManager(IProjectStorage storage)
    {
        _storage = storage;
    }

    public async Task LoadAsync()
    {
        if (IsInitialized) return;
        Projects = await _storage.LoadProjectsAsync();
        IsInitialized = true;
        OnProjectsChanged?.Invoke();

    }

    public async Task SaveAsync()
    {
        await _storage.SaveProjectsAsync(Projects);
    }
}
