using HotelApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Context;

public class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}