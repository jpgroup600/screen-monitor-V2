using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Context;
using ScreenshotMonitor.Data.Dto.Screenshot;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.Data.Repositories;

public class ScreenshotRepository(
    IConfiguration conf,
    SmDbContext dbContext,
    ILogger<ScreenshotRepository> logger
) : IScreenshotRepository
{
    private readonly SmDbContext _dbContext = dbContext;
    private readonly ILogger<ScreenshotRepository> _logger = logger;
    /*
    private readonly string _storagePath = conf["ScreenshotStoragePath"] ?? "Screenshots";
    */
    private readonly string _storagePath = @"C:\\Users\\ahsan\\Desktop\\ScreenshotMonitor LocalStorage\\screenshots";
    public async Task<bool> UploadScreenshotDuringSessionAsync(string employeeId, IFormFile image)
{
    try
    {
        var session = await _dbContext.Sessions
            .Where(s => s.EmployeeId == employeeId && s.Status == "Active")
            .OrderByDescending(s => s.StartTime)
            .FirstOrDefaultAsync();

        if (session == null)
        {
            _logger.LogWarning("No active session found for Employee {EmployeeId}, screenshot upload failed.", employeeId);
            return false;
        }

        if (image == null || image.Length == 0)
        {
            _logger.LogWarning("Invalid image file received for Employee {EmployeeId}.", employeeId);
            return false;
        }

        var allowedExtensions = new HashSet<string> { ".png", ".jpg", ".jpeg" };
        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            _logger.LogWarning("Unsupported file format {Extension} for Employee {EmployeeId}. Allowed: .png, .jpg, .jpeg", fileExtension, employeeId);
            return false;
        }

        string fileName = $"{session.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
        string filePath = Path.Combine(_storagePath, fileName);

        Directory.CreateDirectory(_storagePath); // Ensure directory exists

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var screenshot = new Screenshot
        {
            Id = Guid.NewGuid().ToString(),
            SessionId = session.Id,
            FilePath = filePath,
            CapturedAt = DateTime.UtcNow
        };

        _dbContext.Screenshots.Add(screenshot);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Screenshot uploaded successfully for Employee {EmployeeId} in Session {SessionId}.", employeeId, session.Id);
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error uploading screenshot for Employee {EmployeeId}.", employeeId);
        return false;
    }
}

    
    public async Task<List<string>> FetchScreenshotsBySessionIdAsync(string sessionId)
    {
        try
        {
            string sessionPath = "C:\\Users\\ahsan\\Desktop\\ScreenshotMonitor LocalStorage\\screenshots";

            if (!Directory.Exists(sessionPath))
            {
                return new List<string>(); // Return empty if directory doesn't exist
            }

            // Get all images matching the sessionId with extensions png, jpg, jpeg
            var imageFiles = Directory.GetFiles(sessionPath)
                .Where(file => Path.GetFileName(file).StartsWith(sessionId) &&
                               (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg")))
                .ToList();

            return imageFiles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching screenshots for session: {SessionId}", sessionId);
            throw;
        }
    }

    public async Task<List<ScreenshotDto>> GetScreenshotsBySessionIdAsync(string sessionId)
    {
        try
        {
            // The directory where all screenshots are stored
            if (!Directory.Exists(_storagePath))
            {
                _logger.LogWarning("No screenshot directory found at: {StoragePath}", _storagePath);
                return new List<ScreenshotDto>();
            }

            // Search for images in the main directory that start with sessionId
            var imageFiles = Directory.GetFiles(_storagePath, $"{sessionId}_*.*") // Match sessionId prefix
                .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                .Select(file => new ScreenshotDto()
                {
                    FilePath = file, // Full file path
                    FileName = Path.GetFileName(file), // Extract filename only
                    CreatedAt = File.GetCreationTime(file) // Get creation time
                })
                .ToList();

            _logger.LogInformation("Fetched {Count} screenshots for session: {SessionId}", imageFiles.Count, sessionId);
            return imageFiles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching screenshots for session: {SessionId}", sessionId);
            throw;
        }
    }


// Helper method to get MIME type
    private static string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
    
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream" // Default for unknown file types
        };
    }


}
