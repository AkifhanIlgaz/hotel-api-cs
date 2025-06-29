using System.Net;
using System.Text.Json;
using HotelApi.Exceptions;
using HotelApi.Responses;

namespace HotelApi.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Global Exception Handler yakaladı: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Bir hata oluştu"
        };

        // Exception türüne göre status code ve mesaj belirle
        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Doğrulama hatası";
                response.Errors = validationEx.Errors;
                break;

            case NotFoundException notFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = "Kaynak bulunamadı";
                response.Errors.Add(notFoundEx.Message);
                break;

            case UnauthorizedException unauthorizedEx:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = "Yetkilendirme hatası";
                response.Errors.Add(unauthorizedEx.Message);
                break;

            case BusinessException businessEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "İş kuralı hatası";
                response.Errors.Add(businessEx.Message);
                break;

            case ArgumentException argEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Geçersiz parametre";
                response.Errors.Add(argEx.Message);
                break;

            case KeyNotFoundException keyNotFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = "Anahtar bulunamadı";
                response.Errors.Add(keyNotFoundEx.Message);
                break;

            case TimeoutException timeoutEx:
                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                response.Message = "İstek zaman aşımına uğradı";
                response.Errors.Add(timeoutEx.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Sunucu hatası";

                // Production'da detaylı hata mesajını gösterme
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    response.Errors.Add(exception.Message);
                    response.Errors.Add(exception.StackTrace ?? "Stack trace yok");
                }
                else
                {
                    response.Errors.Add("Beklenmeyen bir hata oluştu");
                }
                break;
        }

        // JSON seçenekleri
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(jsonResponse);
    }
}
