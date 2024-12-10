#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.DbEnums;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerProgressHistory : BaseEntity<int>
{
    public PlayerProgressHistory()
    {

    }

    public PlayerProgressHistory(int progress, int playerId, string coinId)
    {
        Progress = progress;
        PlayerId = playerId;
        CoinId = coinId;
    }

    public int Progress { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string CoinId { get; private set; }
    public Coin Coin { get; private set; }
}