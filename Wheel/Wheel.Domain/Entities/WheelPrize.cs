using Shared.Domain.Abstractions;

namespace Wheel.Domain.Entities;

public class WheelPrize : BasePrize<WheelPrizeGroup>
{
    public int? WheelIndex { get; set; }
}