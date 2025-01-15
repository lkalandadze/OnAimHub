#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;

namespace Wheel.Domain.Entities;

[CheckMate<WheelPrizeChecker>]
public class WheelPrize : BasePrize<WheelPrizeGroup>
{
    public WheelPrize()
    {

    }

    public WheelPrize(string name, int? prizeGroupId = null, int? wheelIndex = null) : base()
    {
        Name = name;
        WheelIndex = wheelIndex;

        if (prizeGroupId != null)
        {
            PrizeGroupId = prizeGroupId.Value;
        }
    }

    public string Name { get; set; }
    public int? WheelIndex { get; set; }
}