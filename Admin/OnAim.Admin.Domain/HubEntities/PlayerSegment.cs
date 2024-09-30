#nullable disable

namespace OnAim.Admin.Domain.HubEntities;

public class PlayerSegment : BaseEntity<int>
{
    public PlayerSegment()
    {
        
    }

    public PlayerSegment(int playerId, string segmentId)
    {
        PlayerId = playerId;
        SegmentId = segmentId;
    }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string SegmentId { get; private set; }
    public Segment Segment { get; private set; }
}