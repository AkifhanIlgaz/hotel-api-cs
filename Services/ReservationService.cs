using HotelApi.Context;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services;

public class ReservationService(HotelDbContext context) : IReservationRepository
{
    private readonly HotelDbContext _context = context;

    public async Task CreateReservationAsync(Reservation reservation)
    {
        _context.Add(reservation);
        await _context.SaveChangesAsync();
    }

    public Task DeleteReservationAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Reservation> GetReservationByIdAsync(string id)
    {
        return await _context.Reservations.SingleOrDefaultAsync(r => r.Id == id) ??
               throw new KeyNotFoundException($"Reservation with ID {id} not found.");
    }

    public Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(string userId)
    {
        return await _context.Reservations
        .Where(r => r.UserId == userId)
        .Include(r => r.Hotel) // Include hotel details if needed
        .ToListAsync();
    }

    public Task<Reservation> UpdateReservationAsync(Reservation reservation)
    {
        throw new NotImplementedException();
    }

    Task<Reservation> IReservationRepository.CreateReservationAsync(Reservation reservation)
    {
        throw new NotImplementedException();
    }
}