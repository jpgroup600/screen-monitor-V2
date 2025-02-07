using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.SignalR;
public class UserActivityHub : Hub
{
    private static readonly ConcurrentDictionary<string, (string ConnectionId, string Role)> OnlineUsers = new();
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<ScreenHub> _logger;

    // Injecting dependencies for Session Repository and Logger
    public UserActivityHub(ISessionRepository sessionRepository, ILogger<ScreenHub> logger)
    {
        _sessionRepository = sessionRepository;
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"****************************************************************: {Context.ConnectionId}");

        Console.WriteLine($"New Connection: {Context.ConnectionId}");
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
        Console.WriteLine($"User: {userId}, Role: {role}");

        if (!string.IsNullOrEmpty(userId))
        {
            OnlineUsers[userId] = (Context.ConnectionId, role);
            await Clients.All.SendAsync("UserStatusChanged", userId, role, true);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userEntry = OnlineUsers.FirstOrDefault(u => u.Value.ConnectionId == Context.ConnectionId);
        
        if (!string.IsNullOrEmpty(userEntry.Key))
        {
            string employeeId = userEntry.Key;
            OnlineUsers.TryRemove(employeeId, out _);

            await Clients.All.SendAsync("UserStatusChanged", employeeId, userEntry.Value.Role, false);
            _logger.LogInformation("User {EmployeeId} disconnected.", employeeId);

            try
            {
                // End active project ID for the employee
                var projectId = await _sessionRepository.EndSessionAutoOnDisconnectAsync(employeeId, "Active");

                if (!string.IsNullOrEmpty(projectId))
                {
                    _logger.LogInformation("Ending active session for Employee {EmployeeId} in Project {ProjectId}", employeeId, projectId);
                    await _sessionRepository.EndSessionAsync(employeeId, projectId);
                }
                else
                {
                    _logger.LogWarning("No active session found for Employee {EmployeeId} on disconnect.", employeeId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ending session for Employee {EmployeeId} on disconnect.", employeeId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = OnlineUsers.Select(u => new { u.Key, Role = u.Value.Role }).ToList();
        await Clients.Caller.SendAsync("ReceiveOnlineUsers", onlineUsers);
    }
}