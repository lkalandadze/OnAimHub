#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;
using System.Text.Json.Serialization;

namespace Wheel.Domain.Entities;

[CheckMate<WheelPrizeGroupChecker>]
public class WheelPrizeGroup : BasePrizeGroup<WheelPrize>
{
    public WheelPrizeGroup()
    {

    }

    public WheelPrizeGroup(string name, IEnumerable<WheelPrize> prizes = null)
    {
        Name = name;
        Prizes = prizes.ToList() ?? [];
    }

    public string Name { get; set; }

    [JsonIgnore]
    public WheelConfiguration Configuration { get; set; }
}