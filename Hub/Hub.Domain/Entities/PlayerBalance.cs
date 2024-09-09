#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerBalance : BaseEntity<int>
{
    public PlayerBalance()
    {
        
    }

    public PlayerBalance(decimal amount, int playerId, string currencyId)
    {
        Amount = amount;
        PlayerId = playerId;
        CurrencyId = currencyId;
    }

    public decimal Amount { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public void SetAmount(decimal amount)
    {
        Amount = amount;
    }
}