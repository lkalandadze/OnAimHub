namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerSegmentAct : BaseEntity<Int32>	{
		public Int32 TotalPlayers { get; set; }
		public Nullable<Int32> ByUserId { get; set; }
		public Boolean IsBulk { get; set; }
		public Nullable<Int32> ActionId { get; set; }
		public PlayerSegmentActType Action { get; set; }
		public String SegmentId { get; set; }
		public Segment Segment { get; set; }
	}
}
