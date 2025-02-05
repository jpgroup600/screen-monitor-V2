using System.Collections.Generic;
using System.Threading.Tasks;
using ScreenshotMonitor.Data.Entities;

namespace ScreenshotMonitor.Data.Interfaces.Repositories;

public interface ISessionRepository
{
    Task<Session> StartSessionAsync(string employeeId, string projectId);
    Task<bool> EndSessionAsync(string employeeId, string projectId);
    Task<IEnumerable<Session>> GetSessionsAsync(string employeeId, string projectId);
    Task<bool> DeleteSessionsAsync(string employeeId, string projectId);
}
