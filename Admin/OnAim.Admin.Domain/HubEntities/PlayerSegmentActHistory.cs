#nullable disable

namespace OnAim.Admin.Domain.HubEntities;

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