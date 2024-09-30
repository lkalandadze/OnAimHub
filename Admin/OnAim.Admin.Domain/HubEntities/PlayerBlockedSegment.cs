#nullable disable

namespace OnAim.Admin.Domain.HubEntities;

public class PlayerBlockedSegment : BaseEntity<int>
{
    public PlayerBlockedSegment()
    {

    }

    public PlayerBlockedSegment(int playerId, string segmentId)
    {
        PlayerId = playerId;
        SegmentId = segmentId;
    }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string SegmentId { get; private set; }
    public Segment Segment { get; private set; }
}