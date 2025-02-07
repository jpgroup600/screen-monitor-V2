using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScreenshotMonitor.Data.Dto.Session;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.API.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController(
    ISessionRepository sessionRepo,
    ILogger<SessionController> logger
) : ControllerBase
{
    private readonly ISessionRepository _sessionRepo = sessionRepo;
    private readonly ILogger<SessionController> _logger = logger;

    /// <summary>
    /// Start a new session for an employee in a project.
    /// </summary>
    [Authorize(Roles = "Employee,Admin")]
    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var session = await _sessionRepo.StartSessionAsync(request.EmployeeId, request.ProjectId);
            
            var response = new SessionResponseDto
            {
                SessionId = session.Id,
                EmployeeId = session.EmployeeId,
                ProjectId = session.ProjectId,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                ActiveDuration = session.EndTime.HasValue ? session.EndTime.Value - session.StartTime : TimeSpan.Zero,
                Status = session.Status
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start session for Employee {EmployeeId} in Project {ProjectId}", request.EmployeeId, request.ProjectId);
            return StatusCode(500, "An error occurred while starting the session.");
        }
    }

    /// <summary>
    /// End an active session for an employee in a project.
    /// </summary>
    [Authorize(Roles = "Employee,Admin")]
    [HttpPost("end")]
    public async Task<IActionResult> EndSession([FromBody] EndSessionRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _sessionRepo.EndSessionAsync(request.EmployeeId, request.ProjectId);
            return success ? Ok("Session ended successfully.") : NotFound("No active session found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to end session for Employee {EmployeeId} in Project {ProjectId}", request.EmployeeId, request.ProjectId);
            return StatusCode(500, "An error occurred while ending the session.");
        }
    }

    /// <summary>
    /// Get all sessions for an employee in a project.
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    [HttpGet("get")]
    public async Task<IActionResult> GetSessions(
        [FromQuery] string employeeId, 
        [FromQuery] string projectId, 
        [FromQuery] string? status = null) // Nullable status
    {
        try
        {
            var sessions = await _sessionRepo.GetSessionsByStatusAsync(employeeId, projectId, status);
            var response = sessions.Select(session => new SessionResponseDto
            {
                SessionId = session.Id,
                EmployeeId = session.EmployeeId,
                ProjectId = session.ProjectId,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                ActiveDuration = session.EndTime.HasValue ? session.EndTime.Value - session.StartTime : TimeSpan.Zero,
                Status = session.Status
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve sessions for Employee {EmployeeId} in Project {ProjectId}", employeeId, projectId);
            return StatusCode(500, "An error occurred while fetching the sessions.");
        }
    }


    /// <summary>
    /// Delete all sessions for an employee in a project.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteSessions([FromQuery] string employeeId, [FromQuery] string projectId)
    {
        try
        {
            var success = await _sessionRepo.DeleteSessionsAsync(employeeId, projectId);
            return success ? Ok("Sessions deleted successfully.") : NotFound("No sessions found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete sessions for Employee {EmployeeId} in Project {ProjectId}", employeeId, projectId);
            return StatusCode(500, "An error occurred while deleting the sessions.");
        }
    }
}
