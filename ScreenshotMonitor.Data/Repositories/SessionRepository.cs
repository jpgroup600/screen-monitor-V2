using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Context;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.Data.Repositories;

public class SessionRepository(
    IConfiguration conf,
    SmDbContext dbContext,
    ILogger<SessionRepository> logger
) : ISessionRepository
{
    private readonly SmDbContext _dbContext = dbContext;
    private readonly ILogger<SessionRepository> _logger = logger;

    public async Task<Session> StartSessionAsync(string employeeId, string projectId)
    {
        try
        {
            var session = new Session
            {
                EmployeeId = employeeId,
                ProjectId = projectId,
                StartTime = DateTime.UtcNow,
                ActiveDuration = TimeSpan.Zero,
                Status = "Active"
            };

            await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Session started for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting session for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            throw;
        }
    }

    public async Task<bool> EndSessionAsync(string employeeId, string projectId)
    {
        try
        {
            var session = await _dbContext.Sessions
                .Where(s => s.EmployeeId == employeeId && s.ProjectId == projectId && s.Status == "Active")
                .OrderByDescending(s => s.StartTime)
                .FirstOrDefaultAsync();

            if (session == null)
            {
                _logger.LogWarning("No active session found for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
                return false;
            }

            session.EndTime = DateTime.UtcNow;
            session.ActiveDuration = session.EndTime.Value - session.StartTime;
            session.Status = "Complete";

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Session ended for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending session for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            throw;
        }
    }

    public async Task<IEnumerable<Session>> GetSessionsAsync(string employeeId, string projectId)
    {
        try
        {
            return await _dbContext.Sessions
                .Where(s => s.EmployeeId == employeeId && s.ProjectId == projectId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sessions for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            throw;
        }
    }

    public async Task<bool> DeleteSessionsAsync(string employeeId, string projectId)
    {
        try
        {
            var sessions = await _dbContext.Sessions
                .Where(s => s.EmployeeId == employeeId && s.ProjectId == projectId)
                .ToListAsync();

            if (!sessions.Any())
            {
                _logger.LogWarning("No sessions found for deletion for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
                return false;
            }

            _dbContext.Sessions.RemoveRange(sessions);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted all sessions for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sessions for Employee {EmployeeId} on Project {ProjectId}", employeeId, projectId);
            throw;
        }
    }
}

