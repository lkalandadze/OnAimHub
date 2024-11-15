using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOption : BaseEntity<int>
{
    public WithdrawOption(string title, string description, string imageUrl, string endpoint, string endpointContent)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
    }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public string Endpoint { get; private set; }
    public string EndpointContent { get; private set; }

    public void Update(string title, string description, string imageUrl, string endpoint, string endpointContent)
    {
        Title = Title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
    }
}
