using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class PrizeType : BaseEntity<int>
{
    public string Name { get; set; }
    public bool IsMultipled { get; set; }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }
}