#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;

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

    public string Name { get; private set; }
    public WheelConfiguration Configuration { get; private set; }
    public new ICollection<WheelPrize> Prizes { get; set; }
}