using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs;

public record HotelSearchRequest
{
    [Required(ErrorMessage = "Page is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
    [DefaultValue(1)]
    public int Page { get; set; }

    [Required(ErrorMessage = "Page size is required.")]
    [Range(1, 10, ErrorMessage = "Page size must be between 1 and 10.")]
    [DefaultValue(10)]
    public int PageSize { get; init; }
    public string? City { get; set; }
    public string? Name { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
    public List<string>? Features { get; set; }
}