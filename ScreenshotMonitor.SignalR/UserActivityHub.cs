using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ScreenshotMonitor.SignalR;
public class UserActivityHub : Hub
{
    private static readonly ConcurrentDictionary<string, (string ConnectionId, string Role)> OnlineUsers = new();

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
            OnlineUsers.TryRemove(userEntry.Key, out _);
            await Clients.All.SendAsync("UserStatusChanged", userEntry.Key, userEntry.Value.Role, false);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = OnlineUsers.Select(u => new { u.Key, Role = u.Value.Role }).ToList();
        await Clients.Caller.SendAsync("ReceiveOnlineUsers", onlineUsers);
    }
}