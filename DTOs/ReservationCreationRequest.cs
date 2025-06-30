using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs;

public record ReservationCreationRequest
{

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 10)]
    public int GuestCount { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }


    [Required]
    public required string HotelId { get; set; }

    [Required]
    public required string UserId { get; set; }
}