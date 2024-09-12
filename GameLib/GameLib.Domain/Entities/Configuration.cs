using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Configuration : BaseEntity<int>
{
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Price> Prices { get; set; }
    public ICollection<Segment> Segments { get; set; }
}