namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerProgress : BaseEntity<int>
    {
        public int Progress { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public string CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
