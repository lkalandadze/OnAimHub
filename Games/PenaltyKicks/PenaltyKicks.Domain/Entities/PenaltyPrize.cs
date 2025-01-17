#nullable disable

using GameLib.Domain.Abstractions;

namespace PenaltyKicks.Domain.Entities;

public class PenaltyPrize : BasePrize<PenaltyPrizeGroup>
{
    public PenaltyPrize()
    {

    }

    public PenaltyPrize(string name, int? prizeGroupId = null) : base()
    {
        Name = name;

        if (prizeGroupId != null)
        {
            PrizeGroupId = prizeGroupId.Value;
        }
    }

    public string Name { get; set; }
}