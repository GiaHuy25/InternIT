using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Dtos;
using UserManagement.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }
        return Ok(new { token = "your_jwt_token" });
    }
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.FindAsync(model.UserId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (user.PasswordHash != model.CurrentPassword)
        {
            return BadRequest("Current password is incorrect");
        }

        user.PasswordHash = model.NewPassword;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok("Password changed successfully");
    }
}
