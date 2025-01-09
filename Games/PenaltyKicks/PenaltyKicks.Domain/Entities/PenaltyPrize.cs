#nullable disable

using GameLib.Domain.Abstractions;

namespace PenaltyKicks.Domain.Entities;

public class PenaltyPrize : BasePrize<PenaltySeries>
{
    public PenaltyPrize()
    {

    }

    public PenaltyPrize(string name, int? roundId = null) : base()
    {
        Name = name;

        if (roundId != null)
        {
            PrizeGroupId = roundId.Value;
        }
    }

    public string Name { get; set; }
}