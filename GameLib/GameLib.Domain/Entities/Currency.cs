using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Currency : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public ICollection<Price> Prices { get; set; }
    public ICollection<PrizeType> PrizeTypes { get; set; }
}