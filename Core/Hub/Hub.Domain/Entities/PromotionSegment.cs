using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PromotionSegment : BaseEntity<int>
{
    public PromotionSegment()
    {
        
    }

    public PromotionSegment(int promotionId, string segmentId)
    {
        PromotionId = promotionId;
        SegmentId = segmentId;
    }

    public int PromotionId { get; private set; }
    public Promotion Promotion { get; private set; }

    public string SegmentId { get; private set; }
    public Segment Segment { get; private set; }
}
