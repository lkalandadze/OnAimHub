namespace OnAim.Admin.Infrasturcture.Entities
{
    public class RejectedLog
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Action { get; set; }
        public int UserId { get; set; }
        public int ObjectId { get; set; }
        public string Log { get; set; }
        public string ErrorMessage { get; set; }
        public int RetryCount { get; set; }
    }
}
