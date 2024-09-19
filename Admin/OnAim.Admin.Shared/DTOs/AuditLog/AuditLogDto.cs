namespace OnAim.Admin.Shared.DTOs.AuditLog
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Log { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
