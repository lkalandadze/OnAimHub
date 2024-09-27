#nullable disable

using Hub.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerProgress : BaseEntity<int>
{
    public PlayerProgress()
    {
        
    }

    public PlayerProgress(int progress, int playerId, string currencyId)
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

    public void SetProgress(int progress)
    {
        Progress = progress;
    }
}