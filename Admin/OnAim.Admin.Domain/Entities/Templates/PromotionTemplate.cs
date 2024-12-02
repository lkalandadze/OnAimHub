using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class PromotionTemplate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? TotalCost { get; set; }
    public PromotionStatus Status { get;  set; }
    public DateTimeOffset StartDate { get;  set; }
    public DateTimeOffset EndDate { get;  set; }

    public ICollection<string> SegmentIds { get;  set; }
    public ICollection<string> CoinIds { get;  set; }
}
