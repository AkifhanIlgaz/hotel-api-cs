using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HotelApi.Models;

public class User : IdentityUser
{
    public virtual ICollection<Reservation> Reservations { get; set; }
}