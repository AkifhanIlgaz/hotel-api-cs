using HotelApi.Models;

namespace HotelApi.DTOs;


public record HotelPaginatedResponse
{
    public IEnumerable<Hotel> Hotels { get; set; } = [];
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}