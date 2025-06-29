using HotelApi.Models;

namespace HotelApi.Repositories;


public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAllAsync();

    Task<Hotel> GetByIdAsync(string id);
}