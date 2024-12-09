#nullable disable

using GameLib.Domain.Entities;

namespace Wheel.Domain.Entities;

public class WheelConfiguration : GameConfiguration<WheelConfiguration>
{
    public WheelConfiguration()
    {
        
    }

    public WheelConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<Round> rounds = null)
        : base(name, value, promotionId, correlationId, templateId, prices)
    {
        Rounds = rounds.ToList() ?? [];
    }

    public ICollection<Round> Rounds { get; set; }
}