using GameLib.Application.Exceptions.Types;

namespace GameLib.Application.Exceptions;

public class ApiException : Exception
{
    public ApiExceptionCodeTypes Code { get; set; }
    public ApiException() : base() { }

    public ApiException(ApiExceptionCodeTypes code = ApiExceptionCodeTypes.Unhandled, string? message = null) : base(message)
    {
        Code = code;
    }
}