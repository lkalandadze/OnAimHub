#nullable disable

using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
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