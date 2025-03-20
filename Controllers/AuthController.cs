using ChatAppBackEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ChatAppDbContext _context;
    private readonly JwtServices _jwtService;

    public AuthController(ChatAppDbContext context, JwtServices jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Invalid email or password");
        }

        // ✅ Pass both user ID and username
        var token = _jwtService.GenerateToken(user.Id, user.Email);
        return Ok(new { token });
    }

    // ✅ Add Signup Endpoint
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] User request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            Name = request.Name,
            Password = request.Password,
            Email = request.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // ✅ Pass both user ID and username
        var token = _jwtService.GenerateToken(user.Id, user.Email);
        return Ok(new { token });
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Select(u => new { u.Id, u.Name, u.Email })
            .ToListAsync();

        return Ok(users);
    }
}
