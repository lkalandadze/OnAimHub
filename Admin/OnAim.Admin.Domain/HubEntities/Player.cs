namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class Player : BaseEntity<Int32>	{
		public string? UserName { get; set; }
		public Nullable<Int32> ReferrerId { get; set; }
		public Boolean HasPlayed { get; set; }
		public Nullable<DateTimeOffset> RegistredOn { get; set; }
		public Nullable<DateTimeOffset> LastVisitedOn { get; set; }
		public ICollection<PlayerBalance> PlayerBalances { get; set; }
		public ICollection<PlayerSegment> PlayerSegments { get; set; }
		public ICollection<PlayerBlockedSegment> PlayerBlockedSegments { get; set; }
	}
}
