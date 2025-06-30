using HotelApi.DTOs;
using HotelApi.Models;

namespace HotelApi.Repositories;


public interface IReservationRepository
{
    // Create a new reservation
    Task<Reservation> CreateReservationAsync(Reservation reservation);

    // Get a reservation by ID
    Task<Reservation> GetReservationByIdAsync(string id);

    // Get all reservations for a specific hotel
    Task<IEnumerable<Reservation>> GetReservationsByHotelIdAsync(string hotelId);

    // Get all reservations for a specific user
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(string userId);

    // Update an existing reservation
    Task<Reservation> UpdateReservationAsync(Reservation reservation);

    // Delete a reservation
    Task DeleteReservationAsync(string id);


}