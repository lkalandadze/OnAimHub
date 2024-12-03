using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public static WithdrawOption ConvertToEntity(CreateWithdrawOptionModel model)
    {
        return new()
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Endpoint = model.Endpoint,
            ContentType = model.ContentType,
            EndpointContent = model.EndpointContent,
        };
    }
}