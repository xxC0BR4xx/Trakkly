using System.Text.Json;
using Trakkly.Shared.Models;

namespace Trakkly.Services;

public class MauiProjectStorage : IProjectStorage
{
    private readonly string _filePath;

    public MauiProjectStorage()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
        System.Diagnostics.Debug.WriteLine("Directory: " + FileSystem.AppDataDirectory);
    }

    public async Task<List<TimerProject>> LoadProjectsAsync()
    {
        if (File.Exists(_filePath))
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<TimerProject>>(json) ?? new List<TimerProject>();
        }
        return new List<TimerProject>();
    }

    public async Task SaveProjectsAsync(List<TimerProject> projects)
    {
        var json = JsonSerializer.Serialize(projects);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
