namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class Player : BaseEntity<int>
    {
        public string? UserName { get; set; }
        public int? ReferrerId { get; set; }
        public bool HasPlayed { get; set; }
        public DateTimeOffset? RegistredOn { get; set; }
        public DateTimeOffset? LastVisitedOn { get; set; }
        public bool IsBanned { get; private set; }
        public ICollection<PlayerBalance> PlayerBalances { get; set; }
        public ICollection<Segment> Segments { get; private set; }
        public ICollection<Segment> BlockedSegments { get; private set; }
    }
}