namespace HotelApi.Responses;

public record LoginResponse
{
    public required string AccessToken { get; set; }
    public required int UserId { get; set; }
}