using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Domain.Entities;

public class AuditLog
{
    public AuditLog()
    {
        
    }
    public AuditLog(string action, int? objectId, string? objectt, int userId, string log, string category)
    {
        Action = action;
        ObjectId = objectId;
        Object = objectt;
        UserId = userId;
        Log = log;
        Category = category;
        Timestamp = SystemDate.Now;
    }

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
