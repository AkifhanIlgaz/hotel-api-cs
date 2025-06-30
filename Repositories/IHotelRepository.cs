using HotelApi.DTOs;
using HotelApi.Models;

namespace HotelApi.Repositories;


public interface IHotelRepository
{
    Task<HotelPaginatedResponse> GetAllAsync(HotelSearchRequest req);
    Task<Hotel> GetByIdAsync(string id);
    Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId);
    Task AddHotelAsync(Hotel hotel);
}