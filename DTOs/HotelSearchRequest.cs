namespace HotelApi.DTOs;

public record HotelSearchRequest
{

    public string? City { get; set; }
    public string? Name { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }
    public List<string>? Features { get; set; }
}