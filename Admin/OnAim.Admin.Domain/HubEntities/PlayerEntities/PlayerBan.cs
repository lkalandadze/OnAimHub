namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerBan : BaseEntity<int>
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public DateTimeOffset DateBanned { get; set; }
        public DateTimeOffset? ExpireDate { get; set; }
        public bool IsPermanent { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset? RevokeDate { get; set; }
        public string Description { get; set; }
    }
}
