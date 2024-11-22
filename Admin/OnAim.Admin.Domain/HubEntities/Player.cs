namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class Player : BaseEntity<Int32>	{
		public string? UserName { get; set; }
		public Nullable<Int32> ReferrerId { get; set; }
		public Boolean HasPlayed { get; set; }
		public Nullable<DateTimeOffset> RegistredOn { get; set; }
		public Nullable<DateTimeOffset> LastVisitedOn { get; set; }
        public bool IsBanned { get; private set; }
        public ICollection<PlayerBalance> PlayerBalances { get; set; }
        public ICollection<Segment> Segments { get; private set; }
        public ICollection<Segment> BlockedSegments { get; private set; }
    }
}