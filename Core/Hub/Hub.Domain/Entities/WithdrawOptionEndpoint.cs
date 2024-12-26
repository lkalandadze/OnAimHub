#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionEndpoint : BaseEntity<int>
{
    public WithdrawOptionEndpoint()
    {

    }

    public WithdrawOptionEndpoint(
        string name, 
        string endpoint, 
        string endpointContent, 
        EndpointContentType contentType, 
        int? createdByUserId = null)
    {
        Name = name;
        Endpoint = endpoint;
        Content = endpointContent;
        ContentType = contentType;
        CreatedByUserId = createdByUserId;
    }

    public string Name { get; private set; }
    public string Endpoint { get; private set; }
    public EndpointContentType ContentType { get; private set; }
    public string Content { get; set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset DateDeleted { get; private set; }
    public int? CreatedByUserId { get; private set; }

    public void Update(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        Content = endpointContent;
        ContentType = contentType;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}