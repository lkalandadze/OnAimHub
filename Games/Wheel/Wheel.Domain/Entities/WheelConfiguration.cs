#nullable disable

using GameLib.Domain.Entities;

namespace Wheel.Domain.Entities;

public class WheelConfiguration : GameConfiguration<WheelConfiguration>
{
    public WheelConfiguration()
    {
        
    }

    public WheelConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<WheelPrizeGroup> prizeGroups = null)
        : base(name, value, promotionId, correlationId, templateId, prices)
    {
        IsActive = true;
        WheelPrizeGroups = prizeGroups.ToList() ?? [];
    }

    public ICollection<WheelPrizeGroup> WheelPrizeGroups { get; set; }
}