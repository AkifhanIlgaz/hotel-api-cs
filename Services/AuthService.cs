using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using HotelApi.Context;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelApi.Services;


public class AuthService(HotelDbContext context, IConfiguration configuration) : IAuthService
{
    private readonly HotelDbContext _context = context;
    private readonly IConfiguration _config = configuration;

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>{
                 new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                 new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new(ClaimTypes.Name, user.Name),
                 new(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        double.TryParse(_config["Jwt:ExpirationMinutes"], out double expirationMinutes);
        var expires = DateTime.Now.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
                   issuer: _config["Jwt:Issuer"],
                   audience: _config["Jwt:Audience"],
                   claims: claims,
                   expires: expires,
                   signingCredentials: creds
               );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> RegisterUserAsync(RegisterRequest req)
    {
        // Check if user exists
        if (await _context.Users.AnyAsync(u => u.Name == req.Name || u.Email == req.Email)) return null;

        var user = new User
        {
            Email = req.Email,
            Name = req.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public async Task<User?> ValidateUserCredentialsAsync(LoginRequest req)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return null;

        return user;
    }
}