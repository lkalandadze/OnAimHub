namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerProgress : BaseEntity<int>
    {
        public int Progress { get; private set; }

        public int PlayerId { get; private set; }
        public Player Player { get; private set; }

        public string CoinId { get; private set; }
        public Coin.Coin Coins { get; private set; }
    }
}
