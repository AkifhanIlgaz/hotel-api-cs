using HotelApi.Context;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services;

public class HotelService(HotelDbContext context) : IHotelRepository
{
    private readonly HotelDbContext _context = context;

    public async Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return await _context.Hotels.ToListAsync();
    }

    public async Task<Hotel> GetByIdAsync(string id)
    {
        var hotel = await _context.Hotels.FindAsync(id) ?? throw new KeyNotFoundException($"Hotel with ID {id} not found.");
        return hotel;
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId)
    {

        if (!Guid.TryParse(hotelId, out _))
            throw new ArgumentException("Invalid hotel ID format.", nameof(hotelId));

        var reservations = await _context.Reservations
            .Where(r => r.HotelId == hotelId)
            .ToListAsync();

        if (reservations == null || reservations.Count == 0)
            throw new KeyNotFoundException($"No reservations found for hotel with ID {hotelId}.");

        return reservations;
    }



    public async Task<IEnumerable<Hotel>> SearchAsync(HotelSearchRequest req)
    {
        var query = _context.Hotels.AsQueryable();

        if (!string.IsNullOrEmpty(req.City))
            query = query.Where(h => EF.Functions.Like(h.Location, $"%{req.City}%"));

        if (!string.IsNullOrEmpty(req.Name))
            query = query.Where(h => EF.Functions.Like(h.Name, $"%{req.Name}%"));

        if (req.MinPrice.HasValue)
            query = query.Where(h => h.PricePerNight >= req.MinPrice.Value);

        if (req.MaxPrice.HasValue)
            query = query.Where(h => h.PricePerNight <= req.MaxPrice.Value);

        if (req.MinRating.HasValue)
            query = query.Where(h => h.Rating >= req.MinRating.Value);

        if (req.MaxRating.HasValue)
            query = query.Where(h => h.Rating <= req.MaxRating.Value);

        if (req.Features is not null)
        {
            foreach (var feature in req.Features)
            {
                query = query.Where(h => EF.Functions.Like(h.Features, $"%{feature.ToLower()}%"));
            }
        }


        var hotels = await query.ToListAsync();
        return hotels;
    }

    public Task AddHotelAsync(Hotel hotel)
    {
        hotel.Id = Guid.NewGuid().ToString(); // Ensure a new ID is generated
        _context.Hotels.Add(hotel);
        return _context.SaveChangesAsync();
    }
}