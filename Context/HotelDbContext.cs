using HotelApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Context;

public class HotelDbContext(DbContextOptions<HotelDbContext> options) : IdentityDbContext<IdentityUser>(options)
{

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Reservation>()
            .Property(r => r.TotalPrice)
            .HasColumnType("decimal(18, 2)");
    }
}