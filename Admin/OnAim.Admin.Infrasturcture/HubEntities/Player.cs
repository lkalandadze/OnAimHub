namespace NMSPC
{
	// Generated Code

	public class Player : BaseEntity<Int32>	{
		public String UserName { get; set; }
		public Nullable<Int32> ReferrerId { get; set; }
		public Boolean HasPlayed { get; set; }
		public ICollection<PlayerBalance> PlayerBalances { get; set; }
		public ICollection<PlayerSegment> PlayerSegments { get; set; }
	}
}
