using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtMicroservice.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtMicroservice.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwt = _configuration.GetSection("Jwt");
        var keyValue = jwt["Key"]
            ?? throw new InvalidOperationException("JWT key is missing.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

        var credentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var duration = int.TryParse(jwt["DurationInMinutes"], out var minutes)
            ? minutes
            : 60;

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(duration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
