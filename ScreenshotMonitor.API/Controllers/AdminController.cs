using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Dto.Authentication;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Repositories.Interfaces;
using ScreenshotMonitor.Data.Entities.Mapper;

[Route("api/admin")]
[ApiController]
public class AdminController(
    IAuthRepository authRepository,
    ILogger<AdminController> logger
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto user)
    {
        var admin = user.ToUserEntity("Admin"); // Convert DTO to User entity
        var token = await authRepository.RegisterAdmin(admin);
        if (token == null)
            return BadRequest("Failed to register admin.");

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await authRepository.LoginAdmin(loginDto.Email, loginDto.Password);
        if (token == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new { Token = token });
    }
}
