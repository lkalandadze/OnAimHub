using CheckmateValidations;
using Wheel.Domain.Entities;
using Checkmate;

namespace Wheel.Domain.Checkers;

public class RoundChecker : Checkmate<Round>
{
    public RoundChecker() : base()
    {
        //Check(x => x.Name.Length)
        //    .GreaterThan(3)
        //    .WithMessage("The length of the name must be at least 3.")
        //    .LessThan(35)
        //    .WithMessage("The length of the name must be more than 35.");

        //Check(x => x.Prizes)
        //   .SetCondition(x => x.Sum(x => x.Probability) == 100)
        //   .WithMessage("The sum of prize probabilities should be a 100.");
    }
}