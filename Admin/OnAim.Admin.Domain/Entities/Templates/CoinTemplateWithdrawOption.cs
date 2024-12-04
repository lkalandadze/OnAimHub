using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class CoinTemplateWithdrawOption
{
    public CoinTemplateWithdrawOption()
    {

    }

    public CoinTemplateWithdrawOption(string coinTemplateId, int withdrawOptionId)
    {
        Id = ObjectId.GenerateNewId().ToString();
        CoinTemplateId = coinTemplateId;
        WithdrawOptionId = withdrawOptionId;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string CoinTemplateId { get; set; }
    public CoinTemplate CoinTemplate { get; set; }

    public int WithdrawOptionId { get; set; }
    public WithdrawOption WithdrawOption { get; set; }
}