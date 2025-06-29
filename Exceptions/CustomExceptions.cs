namespace HotelApi.Exceptions;

public class BusinessException(string message) : Exception(message)
{
}

public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = [message];
    }

    public ValidationException(List<string> errors) : base("Validation failed")
    {
        Errors = errors;
    }


}

public class NotFoundException(string message) : Exception(message)
{
}

public class UnauthorizedException(string message) : Exception(message)
{
}