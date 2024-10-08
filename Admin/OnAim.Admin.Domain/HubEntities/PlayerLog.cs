namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerLog : BaseEntity<Int32>	{
		public string Log { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public Int32 PlayerLogTypeId { get; set; }
		public PlayerLogType PlayerLogType { get; set; }
	}
}
