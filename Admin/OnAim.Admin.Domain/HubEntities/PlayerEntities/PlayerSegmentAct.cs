namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerSegmentAct : BaseEntity<int>
    {
        public int TotalPlayers { get; set; }
        public int? ByUserId { get; set; }
        public bool IsBulk { get; set; }
        public int? ActionId { get; set; }
        public PlayerSegmentActType Action { get; set; }
        public string SegmentId { get; set; }
        public Segment Segment { get; set; }
    }
}
