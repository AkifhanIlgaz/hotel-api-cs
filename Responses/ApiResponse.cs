namespace HotelApi.Responses;

public enum Status
{
    Success,
    Error
}


public class ApiResponse<T>
{
    public Status Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T? data, string? message = "İşlem başarılı.")
    {
        return new ApiResponse<T>
        {
            Status = Status.Success,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Error(string? message = "Bir hata oluştu.")
    {
        return new ApiResponse<T>
        {
            Status = Status.Error,
            Message = message,
        };
    }
}