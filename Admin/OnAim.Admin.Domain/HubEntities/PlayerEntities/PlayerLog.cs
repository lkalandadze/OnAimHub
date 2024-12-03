namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerLog : BaseEntity<int>
    {
        public string Log { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int PlayerLogTypeId { get; set; }
        public PlayerLogType PlayerLogType { get; set; }
    }
}
