using CheckmateValidations;
using Wheel.Domain.Entities;

namespace Wheel.Domain.Checkers;

public class WheelConfigurationChecker : Checkmate<WheelConfiguration>
{
    public WheelConfigurationChecker() : base()
    {
        Check(x => x.WheelPrizeGroups)
            .SetCondition(x => x.Count > 0)
            .WithMessage("Configuration should have at least 1 wheel prize group.");
    }
}