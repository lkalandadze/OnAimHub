using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public class PrizeType : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public bool IsMultipled { get; set; }

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }

    //public ICollection<BasePrize> Prizes { get; set; }
}