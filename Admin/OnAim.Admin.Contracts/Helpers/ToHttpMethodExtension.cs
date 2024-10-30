using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Contracts.Helpers;

public static class ToHttpMethodExtension
{
    public static string ToHttpMethod(EndpointType? type)
    {
        return type switch
        {
            EndpointType.Get => "GET",
            EndpointType.Create => "POST",
            EndpointType.Update => "PUT",
            EndpointType.Delete => "DELETE",
            _ => "UNKNOWN"
        };
    }
}
