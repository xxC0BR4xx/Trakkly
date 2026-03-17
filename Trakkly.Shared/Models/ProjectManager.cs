using System.Text.Json;
using Trakkly.Shared.Models;
using Microsoft.Maui.Storage;

public class ProjectManager
{
    private readonly string _filePath;

    public List<TimerProject> Projects { get; private set; } = new();

    public ProjectManager()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "projects.json");
    }

    public async Task SaveProjectsAsync()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true, // if you use public fields (rare)
            PropertyNameCaseInsensitive = true
        };

        var json = JsonSerializer.Serialize(Projects, options);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task LoadProjectsAsync()
    {
        if (File.Exists(_filePath))
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var data = JsonSerializer.Deserialize<List<TimerProject>>(json);
            if (data != null)
                Projects = data;
        }
    }
}