#nullable disable

using GameLib.Domain.Entities;

namespace PenaltyKicks.Domain.Entities;

public class PenaltyConfiguration : GameConfiguration<PenaltyConfiguration>
{
    public PenaltyConfiguration()
    {

    }

    public PenaltyConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<PenaltySeries> penaltySeries = null)
        : base(name, value, promotionId, correlationId, templateId, prices)
    {
        PenaltySeries = penaltySeries.ToList() ?? [];
    }

    public int KicksCount { get; set; }
    public ICollection<PenaltySeries> PenaltySeries { get; set; }
}