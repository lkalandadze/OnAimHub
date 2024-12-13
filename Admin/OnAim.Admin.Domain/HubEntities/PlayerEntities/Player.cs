namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class Player : BaseEntity<int>
    {
        public const string Base32Chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        private static int ReferralCodeMargin = 10_000_000;

        public string UserName { get; private set; }
        public int? ReferrerId { get; private set; }
        public bool HasPlayed { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTimeOffset? RegistredOn { get; private set; }
        public DateTimeOffset? LastVisitedOn { get; private set; }

        public ICollection<Segment> Segments { get; private set; }
        public ICollection<Segment> BlockedSegments { get; private set; }
        public ICollection<PlayerBalance> PlayerBalances { get; private set; }
    }
}