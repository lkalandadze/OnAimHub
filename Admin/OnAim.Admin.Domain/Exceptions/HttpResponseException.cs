using Microsoft.AspNetCore.Http;

namespace OnAim.Admin.Domain.Exceptions;

public class HttpResponseException : System.Exception
{
    public string? ResponseContent { get; }

    public IReadOnlyDictionary<string, IEnumerable<string>>? Headers { get; }

    public HttpResponseException(
        string responseContent,
        int statusCode = StatusCodes.Status500InternalServerError,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers = null,
        System.Exception? inner = null
    )
    {
        ResponseContent = responseContent;
        Headers = headers;
    }

    public override string ToString()
    {
        return $"HTTP Response: \n\n{ResponseContent}\n\n{base.ToString()}";
    }
}
