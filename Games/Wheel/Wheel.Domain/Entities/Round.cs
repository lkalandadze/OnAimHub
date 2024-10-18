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

    public string Name { get; set; }
    public WheelConfiguration Configuration { get; set; }
    public ICollection<WheelPrize> Prizes { get; set; }
}