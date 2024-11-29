#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Withdraw.WithdrawOption;

public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public static Domain.Entities.WithdrawOption ConvertToEntity(CreateWithdrawOptionModel model)
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