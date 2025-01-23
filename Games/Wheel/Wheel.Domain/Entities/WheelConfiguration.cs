#nullable disable

using CheckmateValidations;
using GameLib.Domain.Entities;
using Wheel.Domain.Checkers;

namespace Wheel.Domain.Entities;

[CheckMate<WheelConfigurationChecker>]
public class WheelConfiguration : GameConfiguration<WheelConfiguration>
{
    public WheelConfiguration()
    {
        
    }

    public WheelConfiguration(string name, int value, int promotionId, string description = null, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<WheelPrizeGroup> prizeGroups = null)
        : base(name, value, promotionId, description, correlationId, templateId, prices)
    {
        IsActive = true;
        WheelPrizeGroups = prizeGroups.ToList() ?? [];
    }

    public ICollection<WheelPrizeGroup> WheelPrizeGroups { get; set; }
}