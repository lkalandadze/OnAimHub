using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Price : BaseEntity<int>
{
    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public int SegmentId { get; set; }
    public Segment Segment { get; set; }
}