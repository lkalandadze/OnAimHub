using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Segment : BaseEntity<int>
{
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }

    //public ICollection<BasePrizeGroup> PrizeGroups { get; set; }
    public ICollection<Price> Prices { get; set; }
}