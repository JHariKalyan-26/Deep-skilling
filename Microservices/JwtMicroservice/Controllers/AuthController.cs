using JwtMicroservice.Models;
using JwtMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;

    public AuthController(JwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Demo credentials:
        // admin / Admin@123
        // user / User@123
        User? user = (model.Username, model.Password) switch
        {
            ("admin", "Admin@123") =>
                new User { Username = "admin", Role = "Admin" },

            ("user", "User@123") =>
                new User { Username = "user", Role = "User" },

            _ => null
        };

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password."
            });
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            token,
            username = user.Username,
            role = user.Role
        });
    }
}
