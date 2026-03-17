using System.Text.Json;
using Trakkly.Shared.Models;

namespace Trakkly.Web.Services;

using Microsoft.JSInterop;

public class BrowserProjectStorage : IProjectStorage
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "projects";

    public BrowserProjectStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<TimerProject>> LoadProjectsAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(json))
            return new List<TimerProject>();

        return JsonSerializer.Deserialize<List<TimerProject>>(json) ?? new List<TimerProject>();
    }

    public async Task SaveProjectsAsync(List<TimerProject> projects)
    {
        var json = JsonSerializer.Serialize(projects);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}