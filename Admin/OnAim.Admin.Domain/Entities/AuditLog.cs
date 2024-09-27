using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnAim.Admin.Domain.Entities;

public class AuditLog
{
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Action { get; set; }
    public int? ObjectId { get; set; }
    public string? Object { get; set; }
    public int UserId { get; set; }
    public string Log { get; set; }
    public string Category { get; set; }
}
