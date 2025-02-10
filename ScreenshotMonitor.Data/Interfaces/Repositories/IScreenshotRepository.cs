using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ScreenshotMonitor.Data.Dto.Screenshot;

namespace ScreenshotMonitor.Data.Interfaces.Repositories;

public interface IScreenshotRepository
{
    Task<bool> UploadScreenshotDuringSessionAsync(string employeeId, IFormFile image);
    Task<List<string>> FetchScreenshotsBySessionIdAsync(string sessionId);
    Task<List<ScreenshotDto>> GetScreenshotsBySessionIdAsync(string sessionId);
}
