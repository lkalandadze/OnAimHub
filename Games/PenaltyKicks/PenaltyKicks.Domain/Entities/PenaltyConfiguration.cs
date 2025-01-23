#nullable disable

using CheckmateValidations;
using GameLib.Domain.Entities;
using PenaltyKicks.Domain.Checkers;

namespace PenaltyKicks.Domain.Entities;

[CheckMate<PenaltyConfigurationChecker>]
public class PenaltyConfiguration : GameConfiguration<PenaltyConfiguration>
{
    public PenaltyConfiguration()
    {

    }

    public PenaltyConfiguration(string name, int value, int promotionId, string description = null, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<PenaltyPrizeGroup> prizeGroups = null)
        : base(name, value, promotionId, description, correlationId, templateId, prices)
    {
        PenaltyPrizeGroups = prizeGroups.ToList() ?? [];
    }

    public int KicksCount { get; set; }
    public ICollection<PenaltyPrizeGroup> PenaltyPrizeGroups { get; set; }
}