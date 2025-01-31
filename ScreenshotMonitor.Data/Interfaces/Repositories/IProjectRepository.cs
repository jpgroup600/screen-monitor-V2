using System.Collections.Generic;
using System.Threading.Tasks;
using ScreenshotMonitor.Data.Entities;

namespace ScreenshotMonitor.Data.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetProjectByIdAsync(string projectId);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(string employeeId);
    Task<bool> CreateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(string projectId);
    Task<bool> AddEmployeeToProjectAsync(string projectId, string employeeId);
    Task<bool> RemoveEmployeeFromProjectAsync(string projectId, string employeeId);
}
