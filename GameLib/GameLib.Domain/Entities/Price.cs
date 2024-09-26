#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Price : BaseEntity<int>
{
    public Price()
    {
        
    }

    public Price(decimal value, decimal multiplier, string currencyId, int segmentId)
    {
        Value = value;
        Multiplier = multiplier;
        CurrencyId = currencyId;
        SegmentId = segmentId;
    }

    public decimal Value { get; private set; }
    public decimal Multiplier { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public int SegmentId { get; private set; }
    public Configuration Configuration { get; private set; }
}