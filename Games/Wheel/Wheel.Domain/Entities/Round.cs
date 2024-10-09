using GameLib.Domain.Abstractions;

namespace Wheel.Domain.Entities;

public class Round : BasePrizeGroup<WheelPrize>
{
    public Round()
    {
    }

    public string Name { get; set; }
    public ICollection<WheelPrize> Prizes { get; set; }
}