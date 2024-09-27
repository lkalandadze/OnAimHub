using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.Entities;

public class RejectedLog
{
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Action { get; set; }
    public int UserId { get; set; }
    public int? ObjectId { get; set; }
    public string Object { get; set; }
    public string Log { get; set; }
    public string ErrorMessage { get; set; }
    public int RetryCount { get; set; }
}
