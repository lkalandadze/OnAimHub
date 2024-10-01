namespace NMSPC
{
	// Generated Code

	public class PlayerSegment : BaseEntity<Int32>	{
		public Nullable<Int32> AddedByUserId { get; set; }
		public Int32 DeletedByUserId { get; set; }
		public Boolean IsDeleted { get; set; }
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public String SegmentId { get; set; }
		public Segment Segment { get; set; }
	}
}
