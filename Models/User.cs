using Microsoft.AspNetCore.Identity;

namespace HotelApi.Models;

public class User : IdentityUser
{
    public bool IsAdmin { get; set; } = false;
}