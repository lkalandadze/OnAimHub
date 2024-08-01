using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public partial class Base
{
    public class Currency : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public ICollection<Price> Prices { get; set; }
        public ICollection<PrizeType> PrizeTypes { get; set; }
    }
}