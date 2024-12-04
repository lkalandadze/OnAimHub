using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities.Coin;

namespace OnAim.Admin.Domain.Entities.Templates;

public class PromotionTemplate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TimeSpan StartDate { get;  set; }
    public TimeSpan EndDate { get;  set; }

    public ICollection<string> SegmentIds { get;  set; } // დასაზუსტებელია ჭირდება თუ არა სეგმენტები. თუ კი მაშინ ასე იყოს თუ არადა წაშალე.

    public ICollection<Coin> Coins { get;  set; }
}
