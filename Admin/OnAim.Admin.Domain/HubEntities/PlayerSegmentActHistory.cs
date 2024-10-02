namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerSegmentActHistory : BaseEntity<Int32>	{
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public Int32 PlayerSegmentActId { get; set; }
		public PlayerSegmentAct PlayerSegmentAct { get; set; }
	}
}
