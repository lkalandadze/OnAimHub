namespace OnAim.Admin.Infrasturcture.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Action { get; set; }
        public int ObjectId { get; set; }
        public int UserId { get; set; }
        public string Log { get; set; }
        //public string Type { get; set; }
    }
}
