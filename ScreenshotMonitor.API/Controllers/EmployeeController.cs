using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScreenshotMonitor.Data.Dto.Authentication;
using ScreenshotMonitor.Data.Entities.Mapper;
using ScreenshotMonitor.Data.Entities;
using ScreenshotMonitor.Data.Repositories.Interfaces;

[Route("api/employee")]
[ApiController]
public class EmployeeController(
    IAuthRepository authRepository,
    ILogger<EmployeeController> logger
) : ControllerBase
{ 
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto user)
    {
        var employee = user.ToUserEntity("Employee"); // Convert DTO to User entity
        var token = await authRepository.RegisterEmployee(employee);
        if (token == null)
            return BadRequest("Failed to register employee.");

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await authRepository.LoginEmployee(loginDto.Email, loginDto.Password);
        if (token == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new { Token = token });
    }
}
