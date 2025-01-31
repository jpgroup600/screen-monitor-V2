using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Context;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.Data.Repositories;

public class ProjectRepository(
    IConfiguration conf,
    SmDbContext dbContext,
    ILogger<ProjectRepository> logger
) : IProjectRepository
{
    private readonly SmDbContext _dbContext = dbContext;
    private readonly ILogger<ProjectRepository> _logger = logger;

    // ✅ Get project by ID
    public async Task<Project?> GetProjectByIdAsync(string projectId)
    {
        try
        {
            return await _dbContext.Projects
                .Include(p => p.ProjectEmployees)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving project {projectId}: {ex.Message}");
            return null;
        }
    }

    // ✅ Get all projects
    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        try
        {
            return await _dbContext.Projects.Include(p => p.ProjectEmployees).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all projects: {ex.Message}");
            return new List<Project>();
        }
    }

    // ✅ Get projects assigned to a specific employee
    public async Task<IEnumerable<Project>> GetProjectsByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _dbContext.ProjectEmployees
                .Where(pe => pe.EmployeeId == employeeId)
                .Include(pe => pe.Project)
                .Select(pe => pe.Project)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving projects for employee {employeeId}: {ex.Message}");
            return new List<Project>();
        }
    }

    // ✅ Create a new project (Admin only)
    public async Task<bool> CreateProjectAsync(Project project)
    {
        try
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Project {project.Id} created successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating project {project.Id}: {ex.Message}");
            return false;
        }
    }

    // ✅ Delete a project (Admin only)
    public async Task<bool> DeleteProjectAsync(string projectId)
    {
        try
        {
            var project = await _dbContext.Projects.FindAsync(projectId);
            if (project == null)
            {
                _logger.LogWarning($"Project {projectId} not found.");
                return false;
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Project {projectId} deleted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting project {projectId}: {ex.Message}");
            return false;
        }
    }

    // ✅ Add an employee to a project
    public async Task<bool> AddEmployeeToProjectAsync(string projectId, string employeeId)
    {
        try
        {
            var project = await _dbContext.Projects.FindAsync(projectId);
            var employee = await _dbContext.Users.FindAsync(employeeId);

            if (project == null || employee == null)
            {
                _logger.LogWarning($"Project or Employee not found. ProjectId: {projectId}, EmployeeId: {employeeId}");
                return false;
            }

            var existingAssignment = await _dbContext.ProjectEmployees
                .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (existingAssignment != null)
            {
                _logger.LogWarning($"Employee {employeeId} is already assigned to project {projectId}.");
                return false;
            }

            var newAssignment = new ProjectEmployee
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectId,
                EmployeeId = employeeId,
                TotalActiveTime = TimeSpan.Zero
            };

            _dbContext.ProjectEmployees.Add(newAssignment);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Employee {employeeId} added to project {projectId}.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding employee {employeeId} to project {projectId}: {ex.Message}");
            return false;
        }
    }

    // ✅ Remove an employee from a project
    public async Task<bool> RemoveEmployeeFromProjectAsync(string projectId, string employeeId)
    {
        try
        {
            var assignment = await _dbContext.ProjectEmployees
                .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (assignment == null)
            {
                _logger.LogWarning($"No assignment found for Employee {employeeId} in Project {projectId}.");
                return false;
            }

            _dbContext.ProjectEmployees.Remove(assignment);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Employee {employeeId} removed from project {projectId}.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing employee {employeeId} from project {projectId}: {ex.Message}");
            return false;
        }
    }
}
