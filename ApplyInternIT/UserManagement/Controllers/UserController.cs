using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Dtos;
using UserManagement.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }
    // Lấy danh sách người dùng 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        }).ToList();

        return userDtos;
    }

    // xem user theo id
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "User Not Found" });
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        return userDto;
    }

    // update user
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User Not Found" });
        }

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.FullName = dto.FullName;
        user.UpdatedAt = DateTime.Now;

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound(new { message = "User Not Found" });
            }
            else
            {
                throw;
            }
        }

        return Ok(new { message = "Update User successfully!!" });
    }

    // Thêm user vào dtb
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        dto.Id = user.Id;
        dto.CreatedAt = user.CreatedAt;
        dto.UpdatedAt = user.UpdatedAt;

        return CreatedAtAction("GetUser", new { id = user.Id }, dto);
    }

    // Xóa user
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Delete User successfully" });
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
