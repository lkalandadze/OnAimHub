#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;
using System.Text.Json.Serialization;

namespace Wheel.Domain.Entities;

[CheckMate<RoundChecker>]
public class Round : BasePrizeGroup<WheelPrize>
{
    public Round()
    {

    }

    public Round(string name, IEnumerable<WheelPrize> prizes = null)
    {
        Name = name;
        Prizes = prizes.ToList() ?? [];
    }

    public string Name { get; set; }

    [JsonIgnore]
    public WheelConfiguration Configuration { get; set; }
}