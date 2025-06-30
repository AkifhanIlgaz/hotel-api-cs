using HotelApi.Context;
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



    public async Task<IEnumerable<Hotel>> SearchAsync(string? city, string? name, double? minPrice, double? maxPrice, float? minRating, float? maxRating)
    {
        var query = _context.Hotels.AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(h => EF.Functions.Like(h.Location, $"%{city}%"));

        if (!string.IsNullOrEmpty(name))
            query = query.Where(h => EF.Functions.Like(h.Name, $"%{name}%"));

        if (minPrice.HasValue)
            query = query.Where(h => h.PricePerNight >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(h => h.PricePerNight <= maxPrice.Value);

        if (minRating.HasValue)
            query = query.Where(h => h.Rating >= minRating.Value);

        if (maxRating.HasValue)
            query = query.Where(h => h.Rating <= maxRating.Value);

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