namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerSegment : BaseEntity<Int32>	{
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public String SegmentId { get; set; }
		public Segment Segment { get; set; }
	}
}
