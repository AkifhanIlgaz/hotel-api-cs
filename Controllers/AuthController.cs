using Azure.Core;
using HotelApi.Context;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Responses;
using HotelApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;



    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _authService.RegisterUserAsync(request);

        if (user is null)
        {
            return Conflict(ApiResponse<object>.Error("Bu e-posta adresi zaten kayıtlı."));
        }

        return Ok(ApiResponse<object>.Success("Kullanıcı başarıyla kaydedildi."));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _authService.ValidateUserCredentialsAsync(request);
        if (user is null)
        {
            return Unauthorized(ApiResponse<LoginResponse>.Error("Geçersiz e-posta veya şifre."));
        }

        var token = _authService.GenerateJwtToken(user);
        var response = new LoginResponse { AccessToken = token, UserId = user.Id };

        return Ok(ApiResponse<LoginResponse>.Success(response, "Başarıyla giriş yapıldı."));
    }
}