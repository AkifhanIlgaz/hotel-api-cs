using System.Text;
using HotelApi.Context;
using HotelApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
);

// Controller
builder.Services.AddControllers();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // Anahtarı yapılandırma dosyasından okuyun
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Logging.AddConsole(options =>
{
    options.FormatterName = ConsoleFormatterNames.Json; // JSON formatında loglama
});

var app = builder.Build();


app.MapGet("/api/debug/endpoints", (IEnumerable<EndpointDataSource> endpointSources) =>
{
    var sb = new StringBuilder();
    sb.AppendLine("## Tüm Endpointler\n");

    foreach (var endpointSource in endpointSources)
    {
        foreach (var endpoint in endpointSource.Endpoints)
        {
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                sb.AppendLine($"**Yol:** {routeEndpoint.RoutePattern.RawText}");
                sb.AppendLine($"**Metodlar:** {string.Join(", ", endpoint.Metadata.OfType<HttpMethodMetadata>().SelectMany(m => m.HttpMethods ?? new List<string>()))}");
                sb.AppendLine($"**Adı:** {routeEndpoint.DisplayName}");
                sb.AppendLine($"---");
            }
        }
    }
    return Results.Text(sb.ToString(), "text/markdown");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

