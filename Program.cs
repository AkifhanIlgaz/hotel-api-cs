using HotelApi.Context;
using HotelApi.Extensions;
using HotelApi.Models;
using HotelApi.Repositories;
using HotelApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Auth Demo",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Please enter access token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }

    });
});


// EF Core
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HotelDbContext>()
    .AddDefaultTokenProviders().AddApiEndpoints();
builder.Services.AddScoped<IHotelRepository, HotelService>();
builder.Services.AddScoped<IReservationRepository, ReservationService>();

builder.Services.AddControllers();

var app = builder.Build();



app.MapIdentityApi<User>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed the database
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<HotelDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            context.Database.Migrate(); // Ensure the database is created and migrations are applied

            // Check if data already exists
            {
                logger.LogInformation("Seeding database...");
                var testAdmin = new User { UserName = "akif@example.com", Email = "akif@example.com", };
                userManager.AddToRoleAsync(testAdmin, "Admin").GetAwaiter().GetResult();
                // Create Users
                var user1 = new User { UserName = "akif2", Email = "akif@example.com", };

                var result1 = userManager.CreateAsync(user1, "Password123!").GetAwaiter().GetResult();
                var roleResult1 = userManager.AddToRoleAsync(user1, "Admin").GetAwaiter().GetResult();


                if (!roleResult1.Succeeded) throw new Exception(string.Join("\n", "FPOIWJEPOUNSDFSD", roleResult1.Errors.Select(e => e.Description)));
                if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors.Select(e => e.Description)));

                var user2 = new User { UserName = "gemini", Email = "gemini@example.com", };
                var result2 = userManager.CreateAsync(user2, "Password123!").GetAwaiter().GetResult();
                if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors.Select(e => e.Description)));

                // Create Hotels
                var hotel1 = new Hotel { Id = Guid.NewGuid().ToString(), Name = "Grand Hyatt", Description = "A luxurious hotel in the city center.", Location = "New York", ImageUrl = "https://picsum.photos/seed/hotel1/800/600", PricePerNight = 300, Rating = 4.8f, Features = "Pool, Spa, Gym" };
                var hotel2 = new Hotel { Id = Guid.NewGuid().ToString(), Name = "Seaside Resort", Description = "A beautiful resort with ocean views.", Location = "Malibu", ImageUrl = "https://picsum.photos/seed/hotel2/800/600", PricePerNight = 450, Rating = 4.9f, Features = "Beach Access, Restaurant" };
                var hotel3 = new Hotel { Id = Guid.NewGuid().ToString(), Name = "Mountain Lodge", Description = "Cozy lodge with mountain views.", Location = "Aspen", ImageUrl = "https://picsum.photos/seed/hotel3/800/600", PricePerNight = 250, Rating = 4.7f, Features = "Ski-in/Ski-out, Fireplace" };

                context.Hotels.AddRange(hotel1, hotel2, hotel3);
                context.SaveChanges();

                // Create Reservations
                var reservation1 = new Reservation
                {
                    UserId = user1.Id,
                    HotelId = hotel1.Id,
                    CheckInDate = DateTime.UtcNow.AddDays(10),
                    CheckOutDate = DateTime.UtcNow.AddDays(15),
                    GuestCount = 2,
                    TotalPrice = 1500,
                    Status = "paid"
                };

                var reservation2 = new Reservation
                {
                    UserId = user2.Id,
                    HotelId = hotel2.Id,
                    CheckInDate = DateTime.UtcNow.AddDays(20),
                    CheckOutDate = DateTime.UtcNow.AddDays(25),
                    GuestCount = 1,
                    TotalPrice = 2250,
                    Status = "pending"
                };

                context.Reservations.AddRange(reservation1, reservation2);
                context.SaveChanges();

                logger.LogInformation("Database seeding completed.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
app.UseGlobalExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();

