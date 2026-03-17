namespace Trakkly.Shared.Models;

public interface IProjectStorage
{
    Task<List<TimerProject>> LoadProjectsAsync();
    Task SaveProjectsAsync(List<TimerProject> projects);
}