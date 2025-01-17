using Checkmate;
using CheckmateValidations;
using Wheel.Domain.Entities;

namespace Wheel.Domain.Checkers;

public class WheelPrizeGroupChecker : Checkmate<WheelPrizeGroup>
{
    public WheelPrizeGroupChecker() : base()
    {
        Check(x => x.Name)
            .IsNotNull()
            .WithMessage("The name should not be null.");

        Check(x => x.Prizes)
            .SetCondition(x => x.Count > 2)
            .WithMessage("The number of prizes must be greater than 2.");

        Check(x => x.Prizes)
           .SetCondition(x => x.Sum(x => x.Probability) == 100)
           .WithMessage("The sum of prize probabilities should be a 100.");
    }
}