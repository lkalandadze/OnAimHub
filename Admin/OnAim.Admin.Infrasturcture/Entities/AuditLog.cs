namespace OnAim.Admin.Infrasturcture.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string ActionType { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}
