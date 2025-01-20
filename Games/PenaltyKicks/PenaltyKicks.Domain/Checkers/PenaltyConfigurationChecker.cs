using Checkmate;
using CheckmateValidations;
using PenaltyKicks.Domain.Entities;

namespace PenaltyKicks.Domain.Checkers;

public class PenaltyConfigurationChecker : Checkmate<PenaltyConfiguration>
{
    public PenaltyConfigurationChecker() : base()
    {
        Check(x => x.KicksCount)
           .GreaterThan(2)
           .WithMessage("Kicks count should be at least 3.");

        Check(x => x.PenaltyPrizeGroups)
           .SetCondition(x => x.Count > 0)
           .WithMessage("Configuration should have at least 1 penalty prize group.");
    }
}