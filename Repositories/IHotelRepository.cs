using HotelApi.DTOs;
using HotelApi.Models;

namespace HotelApi.Repositories;


public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAllAsync();

    Task<Hotel> GetByIdAsync(string id);
    Task<IEnumerable<Hotel>> SearchAsync(HotelSearchRequest req);

    Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId);

    Task AddHotelAsync(Hotel hotel);
}