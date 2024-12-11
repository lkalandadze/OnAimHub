using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class CoinTemplateWithdrawOptionGroup
{
    public CoinTemplateWithdrawOptionGroup(){}

    public CoinTemplateWithdrawOptionGroup(string coinTemplateId, int withdrawOptionGroupId)
    {
        CoinTemplateId = coinTemplateId;
        WithdrawOptionGroupId = withdrawOptionGroupId;
    }

    //[BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public string CoinTemplateId { get; set; }

    public int WithdrawOptionGroupId { get; set; }
    public WithdrawOptionGroup WithdrawOptionGroup { get; set; }
}
