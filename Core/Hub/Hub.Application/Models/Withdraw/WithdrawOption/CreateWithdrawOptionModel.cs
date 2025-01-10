#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Withdraw.WithdrawOption;

public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Value { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public static Domain.Entities.WithdrawOption ConvertToEntity(CreateWithdrawOptionModel model)
    {
        return new (model.Title, model.Description, model.ImageUrl, model.Value, model.Endpoint, model.ContentType, model.EndpointContent);
    }
}