#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOption : BaseEntity<int>
{
    public WithdrawOption()
    {
    }

    public WithdrawOption(string title, string description, string imageUrl, string endpoint = null, string endpointContent = null, int? withdrawEndpointTemplateId = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = withdrawEndpointTemplateId;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public string Endpoint { get; private set; }
    public string EndpointContent { get; private set; }

    public int? WithdrawEndpointTemplateId { get; set; }
    public WithdrawEndpointTemplate WithdrawEndpointTemplate { get; set; }

    public void Update(string title, string description, string imageUrl, string? endpoint = null, string? endpointContent = null)
    {
        Title = Title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
    }
}