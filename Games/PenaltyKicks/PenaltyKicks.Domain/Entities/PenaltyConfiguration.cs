#nullable disable

using GameLib.Domain.Entities;

namespace PenaltyKicks.Domain.Entities;

public class PenaltyConfiguration : GameConfiguration<PenaltyConfiguration>
{
    public PenaltyConfiguration()
    {

    }

    public PenaltyConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<PenaltyPrizeGroup> prizeGroups = null)
        : base(name, value, promotionId, correlationId, templateId, prices)
    {
        PenaltyPrizeGroups = prizeGroups.ToList() ?? [];
    }

    public int KicksCount { get; set; }
    public ICollection<PenaltyPrizeGroup> PenaltyPrizeGroups { get; set; }
}