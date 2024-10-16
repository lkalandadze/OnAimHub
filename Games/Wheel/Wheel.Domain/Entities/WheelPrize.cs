using GameLib.Domain.Abstractions;

namespace Wheel.Domain.Entities;

public class WheelPrize : BasePrize<Round>
{
    public WheelPrize()
    {

    }

    public string Name { get; set; }
    public int? WheelIndex { get; set; }
    public int? RoundId { get; set; }
}