using CheckmateValidations;
using PenaltyKicks.Domain.Entities;

namespace PenaltyKicks.Domain.Checkers;

public class PenaltyPrizeGroupChecker : Checkmate<PenaltyPrizeGroup>
{
    public PenaltyPrizeGroupChecker() : base()
    {
        Check(x => x.Prizes)
            .SetCondition(x => x.Count() > 2)
            .WithMessage("The number of prizes must be greater than 2.");
    }
}