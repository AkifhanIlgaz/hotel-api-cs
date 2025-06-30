using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs;

public record HotelCreationRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(150, ErrorMessage = "Location cannot be longer than 150 characters.")]
    public required string Location { get; set; }

    [Required(ErrorMessage = "Image URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
    public required string ImageUrl { get; set; }

    [Required(ErrorMessage = "Price per night is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price per night must be greater than zero.")]
    public int PricePerNight { get; set; }

    [Required(ErrorMessage = "Features is required.")]
    public string[] Features { get; set; } = [];
}