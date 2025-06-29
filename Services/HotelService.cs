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
}