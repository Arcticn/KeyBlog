using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AuthService _userJwtService;
    private readonly JWTHelper _jwtHelper;

    public UserController(AuthService userJwtService, JWTHelper jwtHelper)
    {
        _userJwtService = userJwtService;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto newUser)
    {
        if (newUser == null || string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.PasswordHash))
        {
            return BadRequest("用户名或密码为空");
        }
        User tempUser = new User
        {
            Username = newUser.Username,
            PasswordHash = newUser.PasswordHash,
        };
        await _userJwtService.InsertUser(tempUser);
        var token = _userJwtService.GenerateJwtToken(tempUser);
        return Ok(new
        {
            Token = token,
            User = new
            {
                username = tempUser.Username,
                isAdmin = false
            }
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.PasswordHash))
        {
            return BadRequest("用户名或密码为空");
        }
        var tempUser = await _userJwtService.GetUser(user.Username);
        if (tempUser == null)
        {
            return NotFound("未找到用户");
        }
        if (tempUser.PasswordHash != user.PasswordHash)
        {
            return BadRequest("密码不正确");
        }
        var token = _userJwtService.GenerateJwtToken(tempUser);
        return Ok(new
        {
            Token = token,
            User = new
            {
                username = tempUser.Username,
                isAdmin = tempUser.IsAdmin
            }
        });
    }
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminData()
    {
        return Ok("This is admin data.");
    }

    [HttpGet("user")]
    [Authorize]
    public IActionResult GetUserData()
    {
        return Ok("This is user data.");
    }
}