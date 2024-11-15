using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawEndpointTemplate : BaseEntity<int>
{
    public WithdrawEndpointTemplate(string endpoint, string endpointContent)
    {
        Endpoint = endpoint;
        EndpointContent = endpointContent;
    }

    public string Endpoint { get; private set; }
    public string EndpointContent { get; private set; }
    public EndpointContentType ContentType { get; private set; }
}