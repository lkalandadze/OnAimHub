using Shared.Lib.Entities;

namespace Shared.Domain.Entities;

public class PrizeHistory : BaseEntity
{
    public override int Id { get; set; }
    public int PlayerId { get; set; }
    public int PrizeId { get; set; }
    public int Value { get; set; }
    public int GameVersionId { get; set; }
    public int CurrencyId { get; set; }
    public int PrizeTypeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}