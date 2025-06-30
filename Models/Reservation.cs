using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models
{
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]

        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 10)]
        public int GuestCount { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]

        public required string Status { get; set; }

        public required string HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public required string UserId { get; set; }
        public User User { get; set; }
    }
}
