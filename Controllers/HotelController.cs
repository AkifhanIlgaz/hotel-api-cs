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
}