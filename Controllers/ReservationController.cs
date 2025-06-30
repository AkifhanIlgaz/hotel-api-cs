using System.Security.Claims;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Repositories;
using HotelApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers;

[ApiController]
[Route("/api/reservations")]
public class ReservationController(IReservationRepository reservationRepository) : Controller
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    [HttpGet("{reservationId}")]
    [Authorize]
    public async Task<IActionResult> GetReservationById(string reservationId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);

        if (userId != reservation.UserId)
        {
            return Unauthorized("You are not authorized to view this reservation.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationCreationRequest req)
    {
        if (req == null) throw new ArgumentNullException(nameof(req), "Hotel creation request cannot be null.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reservation = new Reservation
        {
            Id = Guid.NewGuid().ToString(),
            CheckInDate = req.CheckInDate,
            CheckOutDate = req.CheckOutDate,
            GuestCount = req.GuestCount,
            TotalPrice = req.TotalPrice,
            Status = req.Status.Trim().ToLower(),
            HotelId = req.HotelId.Trim(),
            UserId = req.UserId.Trim()
        };

        await _reservationRepository.CreateReservationAsync(reservation);
        return CreatedAtAction(nameof(GetReservationById), new { reservationId = reservation.Id }, reservation);
    }

    [HttpGet("/me")]
    public async Task<IActionResult> GetUserReservations()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }

        var reservations = await _reservationRepository.GetReservationsByUserIdAsync(userId);
        return Ok(reservations);
    }

    [HttpDelete("{reservationId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReservation(string reservationId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);

        if (userId != reservation.UserId)
        {
            return Unauthorized("You are not authorized to delete this reservation.");
        }

        await _reservationRepository.DeleteReservationAsync(reservationId);
        return NoContent();
    }


}