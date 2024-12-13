namespace OnAim.Admin.Domain.HubEntities.PlayerEntities
{
    // Generated Code

    public class PlayerBalance : BaseEntity<int>
    {
        public decimal Amount { get; private set; }

        public int PlayerId { get; private set; }
        public Player Player { get; private set; }

        public string CoinId { get; private set; }
        public Coin.Coin Coin { get; private set; }


        public int PromotionId { get; private set; }
        public Promotion Promotion { get; private set; }
    }
}
