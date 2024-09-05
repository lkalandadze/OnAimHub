using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class PrizeHistory : BaseEntity<int>
{
    public int PlayerId { get; set; }
    public int PrizeId { get; set; }
    public int Value { get; set; }
    public int GameVersionId { get; set; }
    public int CurrencyId { get; set; }
    public int PrizeTypeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}