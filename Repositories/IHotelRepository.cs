using HotelApi.Models;

namespace HotelApi.Repositories;


public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAllAsync();

    Task<Hotel> GetByIdAsync(string id);
    Task<IEnumerable<Hotel>> SearchAsync(string? city, string? name, double? minPrice, double? maxPrice, float? minRating, float? maxRating);

    Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId);

    Task AddHotelAsync(Hotel hotel);
}