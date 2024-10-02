#nullable disable

using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerSegmentActHistory : BaseEntity<int>
{
    public PlayerSegmentActHistory()
    {

    }

    public PlayerSegmentActHistory(int playerId, PlayerSegmentAct playerSegmentAct)
    {
        PlayerId = playerId;
        PlayerSegmentAct = playerSegmentAct;
    }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public int PlayerSegmentActId { get; set; }
    public PlayerSegmentAct PlayerSegmentAct { get; set; }
}