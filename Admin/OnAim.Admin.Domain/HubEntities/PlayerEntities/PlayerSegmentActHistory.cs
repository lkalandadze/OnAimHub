namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerSegmentActHistory : BaseEntity<int>
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int PlayerSegmentActId { get; set; }
        public PlayerSegmentAct PlayerSegmentAct { get; set; }
    }
}
