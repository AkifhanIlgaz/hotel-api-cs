using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models;

public class Hotel
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string Location { get; set; }

    public required string ImageUrl { get; set; }

    public required double PricePerNight { get; set; }

    public float Rating { get; set; }

    public required string Features { get; set; } = string.Empty;


}