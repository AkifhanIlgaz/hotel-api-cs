using HotelApi.Models;
using HotelApi.Repositories;
using HotelApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers;

[ApiController]
[Route("/api/hotels")]
public class HotelController(IHotelRepository hotelRepository) : Controller
{
    private readonly IHotelRepository _hotelRepository = hotelRepository;

    [HttpGet]
    public async Task<IActionResult> GetAllHotels()
    {
        var hotels = await _hotelRepository.GetAllAsync();
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotelById(string id)
    {
        if (!Guid.TryParse(id, out _)) throw new ArgumentException("Invalid hotel ID format.", nameof(id));

        var hotel = await _hotelRepository.GetByIdAsync(id.Trim());
        return Ok(hotel);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchHotels([FromQuery] string? city,
                                                  [FromQuery] string? name,
                                                  [FromQuery] double? minPrice,
                                                  [FromQuery] double? maxPrice,
                                                  [FromQuery] float? minRating,
                                                  [FromQuery] float? maxRating)
    {
        var hotels = await _hotelRepository.SearchAsync(city, name, minPrice, maxPrice, minRating, maxRating);
        return Ok(hotels);
    }


    [HttpGet("{hotelId}/reservations")]
    public async Task<IActionResult> GetReservationsByHotelId(string hotelId)
    {
        var reservations = await _hotelRepository.GetReservationsByHotelIdAsync(hotelId.Trim());
        if (reservations == null || !reservations.Any())
        {
            throw new KeyNotFoundException($"No reservations found for hotel with ID {hotelId}.");
        }
        return Ok(reservations);
    }

}