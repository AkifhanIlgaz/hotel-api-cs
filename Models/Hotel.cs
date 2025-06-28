using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Models;

public class Hotel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public double PricePerNight { get; set; }

    [Range(1, 5)]
    public float Rating { get; set; }

    public string Features { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}