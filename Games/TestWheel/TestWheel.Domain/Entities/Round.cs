#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using System.Text.Json.Serialization;
using TestWheel.Domain.Checkers;

namespace TestWheel.Domain.Entities;

[CheckMate<RoundChecker>]
public class Round : BasePrizeGroup<TestWheelPrize>
{
    public Round()
    {

    }

    public Round(string name, IEnumerable<TestWheelPrize> prizes = null)
    {
        Name = name;
        Prizes = prizes.ToList() ?? [];
    }

    public string Name { get; set; }

    [JsonIgnore]
    public TestWheelConfiguration Configuration { get; set; }
}