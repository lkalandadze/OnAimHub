#nullable disable

using OnAim.Admin.Domain.HubEntities.DbEnums;

namespace OnAim.Admin.Domain.HubEntities;

public class PlayerProgressHistory : BaseEntity<int>
{
    public PlayerProgressHistory()
    {

    }

    public PlayerProgressHistory(int progress, int playerId, string currencyId)
    {
        Progress = progress;
        PlayerId = playerId;
        CurrencyId = currencyId;
    }

    public int Progress { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }
}