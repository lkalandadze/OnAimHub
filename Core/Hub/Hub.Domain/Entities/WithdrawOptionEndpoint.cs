#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionEndpoint : BaseEntity<int>
{
    public WithdrawOptionEndpoint()
    {

    }

    public WithdrawOptionEndpoint(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        Content = endpointContent;
        ContentType = contentType;
    }

    public string Name { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string Content { get; set; }

    public void Update(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        Content = endpointContent;
        ContentType = contentType;
    }
}