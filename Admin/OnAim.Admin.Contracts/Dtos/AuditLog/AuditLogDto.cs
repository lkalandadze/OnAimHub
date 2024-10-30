using MongoDB.Bson;

namespace OnAim.Admin.Contracts.Dtos.AuditLog;

public class AuditLogDto
{
    public ObjectId Id { get; set; }
    public string Action { get; set; }
    public string Log { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}
