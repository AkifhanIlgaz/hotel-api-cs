using HotelApi.Context;
using HotelApi.DTOs;
using HotelApi.Models;
using HotelApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Services;

public class HotelService(HotelDbContext context) : IHotelRepository
{
    private readonly HotelDbContext _context = context;

    public async Task<HotelPaginatedResponse> GetAllAsync(HotelSearchRequest req)
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

        var skipCount = (req.Page - 1) * req.PageSize;
        query = query.Skip(skipCount).Take(req.PageSize);

        return new HotelPaginatedResponse
        {
            Hotels = await query.ToListAsync(),
            CurrentPage = req.Page,
            TotalCount = await _context.Hotels.CountAsync(),
            PageSize = req.PageSize
        };
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
            .Where(r => r.Hotel.Id == hotelId)
            .ToListAsync();

        if (reservations == null || reservations.Count == 0)
            throw new KeyNotFoundException($"No reservations found for hotel with ID {hotelId}.");

        return reservations;
    }


    public Task AddHotelAsync(Hotel hotel)
    {
        hotel.Id = Guid.NewGuid().ToString(); // Ensure a new ID is generated
        _context.Hotels.Add(hotel);
        return _context.SaveChangesAsync();
    }

    public async Task DeleteHotelAsync(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new ArgumentException("Invalid hotel ID format.", nameof(id));

        var hotel = await _context.Hotels.FindAsync(id) ?? throw new KeyNotFoundException($"Hotel with ID {id} not found.");
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateHotelAsync(Hotel hotel)
    {
        if (!Guid.TryParse(hotel.Id, out _))
            throw new ArgumentException("Invalid hotel ID format.", nameof(hotel.Id));

        var existingHotel = (await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotel.Id) ?? throw new KeyNotFoundException($"Hotel with ID {hotel.Id} not found.")) ?? throw new KeyNotFoundException($"Hotel with ID {hotel.Id} not found.");
        existingHotel.Name = hotel.Name;
        existingHotel.Description = hotel.Description;
        existingHotel.Location = hotel.Location;
        existingHotel.ImageUrl = hotel.ImageUrl;
        existingHotel.PricePerNight = hotel.PricePerNight;
        existingHotel.Rating = hotel.Rating;
        existingHotel.Features = hotel.Features;

        _context.Hotels.Update(existingHotel);
        await _context.SaveChangesAsync();
    }
}