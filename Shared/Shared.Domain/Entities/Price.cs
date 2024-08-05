using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public class Price : BaseEntity
{
    public override int Id { get; set; }
    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public int SegmentId { get; set; }
    public Segment Segment { get; set; }

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }
}