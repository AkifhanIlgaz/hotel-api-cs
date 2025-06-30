using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Repositories;
using HotelApi.Responses;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> SearchHotels([FromBody] HotelSearchRequest req)
    {
        var hotels = await _hotelRepository.SearchAsync(req);
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

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AddHotel([FromBody] HotelCreationRequest req)
    {
        if (req == null) throw new ArgumentNullException(nameof(req), "Hotel creation request cannot be null.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var hotel = new Hotel
        {
            Id = Guid.NewGuid().ToString(),
            Name = req.Name.Trim(),
            Description = req.Description.Trim(),
            Location = req.Location.Trim(),
            ImageUrl = req.ImageUrl.Trim(),
            PricePerNight = req.PricePerNight,
            Rating = 0,
            Features = string.Join(", ", req.Features.Select(f => f.Trim()))
        };

        await _hotelRepository.AddHotelAsync(hotel);
        return CreatedAtAction(nameof(GetHotelById), new { id = hotel.Id }, hotel);
    }

}