#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;

namespace Wheel.Domain.Entities;

[CheckMate<WheelPrizeChecker>]
public class WheelPrize : BasePrize<Round>
{
    public WheelPrize()
    {

    }

    public WheelPrize(string name, int? roundId = null, int? wheelIndex = null) : base()
    {
        Name = name;
        WheelIndex = wheelIndex;

        if (roundId != null)
        {
            PrizeGroupId = roundId.Value;
        }
    }

    public string Name { get; set; }
    public int? WheelIndex { get; set; }
}