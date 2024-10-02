namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class Segment : BaseEntity<String>	{
		public String Description { get; set; }
		public Int32 PriorityLevel { get; set; }
		public Nullable<Int32> CreatedByUserId { get; set; }
		public Boolean IsDeleted { get; set; }
		public ICollection<PlayerSegment> PlayerSegments { get; set; }
		public ICollection<PlayerBlockedSegment> PlayerBlockedSegments { get; set; }
	}
}
