#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerSegment : BaseEntity<int>
{
    public PlayerSegment()
    {
        
    }

    public PlayerSegment(int playerId, string segmentId, int? addedByUserId = null)
    {
        AddedByUserId = addedByUserId;
        PlayerId = playerId;
        SegmentId = segmentId;
    }

    public int? AddedByUserId { get; private set; }
    public int DeletedByUserId { get; private set; }
    public bool IsDeleted { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string SegmentId { get; private set; }
    public Segment Segment { get; private set; }

    public void DeletePlayerSegment(int deletedByUserId)
    {
        DeletedByUserId = deletedByUserId;
        IsDeleted = true;
    }
}