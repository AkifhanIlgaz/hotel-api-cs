using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}