using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AggregationService.Domain.Entities;

public class LogEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Subscriber { get; set; }
    public string Producer { get; set; }
    public string EventDetails { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
