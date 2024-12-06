#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using TestWheel.Domain.Checkers;

namespace TestWheel.Domain.Entities;

[CheckMate<TestWheelPrizeChecker>]
public class TestWheelPrize : BasePrize<Round>
{
    public TestWheelPrize()
    {
        
    }

    public TestWheelPrize(string name, int? roundId = null, int? wheelIndex = null) : base()
    {
        Name = name;
        Index = wheelIndex;
        PrizeGroupId = roundId.Value;
    }

    public string Name { get; set; }
    public int? Index { get; set; }
}