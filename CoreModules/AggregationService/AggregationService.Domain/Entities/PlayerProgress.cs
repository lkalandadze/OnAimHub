using Shared.Domain.Entities;

namespace AggregationService.Domain.Entities;

public class PlayerProgress : BaseEntity<int>
{
    public PlayerProgress(int playerId, string coinId, int amount)
    {
        PlayerId = playerId;
        CoinId = coinId;
        Amount = amount;
    }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public string CoinId { get; set; }
    public int Amount { get; set; }
    public int ConfigurationId { get; set; }

    public void Update(int amount)
    {
        Amount = amount;
    }
}
