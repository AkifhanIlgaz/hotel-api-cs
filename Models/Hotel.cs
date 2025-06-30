using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models;

public class Hotel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required string Location { get; set; }

    [Required]
    [Url]
    public required string ImageUrl { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price per night must be greater than zero.")]
    public double PricePerNight { get; set; }

    [Range(0, 5)]
    public float Rating { get; set; }

    public required string Features { get; set; }

}