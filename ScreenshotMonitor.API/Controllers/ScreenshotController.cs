using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScreenshotMonitor.Data.Interfaces.Repositories;

namespace ScreenshotMonitor.API.Controllers;

[ApiController]
[Route("api/screenshots")]
public class ScreenshotController(
    IScreenshotRepository screenshotRepo,
    ILogger<ScreenshotController> logger
) : ControllerBase
{
    private readonly IScreenshotRepository _screenshotRepo = screenshotRepo;
    private readonly ILogger<ScreenshotController> _logger = logger;

    private string GetEmployeeIdFromClaims()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("Employee ID not found in claims.");
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> UploadScreenshot(IFormFile image)
    {
        try
        {
            string employeeId = GetEmployeeIdFromClaims();
            bool success = await _screenshotRepo.UploadScreenshotDuringSessionAsync(employeeId, image);

            if (!success)
                return BadRequest("Failed to upload screenshot.");

            return Ok("Screenshot uploaded successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading screenshot.");
            return StatusCode(500, "Internal server error.");
        }
    }
    [HttpGet("screenshots/zip/{sessionId}")]
    public async Task<IActionResult> DownloadAllScreenshotsAsZip(string sessionId)
    {
        try
        {
            var screenshotPaths = await _screenshotRepo.FetchScreenshotsBySessionIdAsync(sessionId);
            if (screenshotPaths == null || !screenshotPaths.Any())
            {
                return NotFound("No screenshots found for this session.");
            }

            using var memoryStream = new MemoryStream();
            using (var zipArchive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var filePath in screenshotPaths)
                {
                    var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    var zipEntry = zipArchive.CreateEntry(Path.GetFileName(filePath), System.IO.Compression.CompressionLevel.Optimal);

                    using var entryStream = zipEntry.Open();
                    await entryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                }
            }

            return File(memoryStream.ToArray(), "application/zip", $"{sessionId}_screenshots.zip");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving screenshots.");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize(Roles = "Employee,Admin")]
    [HttpGet("getScreenshots/{sessionId}")]
    public IActionResult GetScreenshots(string sessionId)
    {
        try
        {
            var imagePaths = _screenshotRepo.FetchScreenshotsBySessionIdAsync(sessionId).Result;

           
            if (imagePaths == null || !imagePaths.Any())
            {
                return NotFound("No screenshots found for the session.");
            }

            List<object> files = new();

            foreach (var imagePath in imagePaths)
            {
                var fileName = Path.GetFileName(imagePath);
                var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(imagePath)); // Read file into memory

                files.Add(new
                {
                    FileName = fileName,
                    ContentType = GetContentType(fileName),
                    FileStream = Convert.ToBase64String(memoryStream.ToArray()) // Base64 Encode Image
                });
            }

            return Ok(files); // Return list of image files
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving screenshots.");
            return StatusCode(500, "An error occurred while retrieving screenshots.");
        } 
        
        static string GetContentType(string filePath)
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


}
