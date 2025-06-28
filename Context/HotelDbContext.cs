using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Context;

public class HotelDbContext(DbContextOptions<HotelDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    
}