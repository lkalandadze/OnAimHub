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

    public WheelPrize(string name, int roundId, int? wheelIndex = null) : base()
    {
        Name = name;
        WheelIndex = wheelIndex;
        PrizeGroupId = roundId;
    }

    public string Name { get; set; }
    public int? WheelIndex { get; set; }
}