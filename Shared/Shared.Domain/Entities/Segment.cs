using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public class Segment : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }

    public ICollection<PrizeGroup> PrizeGroups { get; set; }
    public ICollection<Price> Prices { get; set; }
}