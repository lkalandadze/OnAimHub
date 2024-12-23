using CheckmateValidations;
using Wheel.Domain.Entities;
using Checkmate;
using System.Linq;

namespace Wheel.Domain.Checkers;

public class RoundChecker : Checkmate<Round>
{
    public RoundChecker() : base()
    {
        Check(x => x.Name)
            .IsNotNull()
            .WithMessage("The name should not be null.");

        Check(x => x.Prizes.Count)
            .GreaterThan(2)
            .WithMessage("The number of prizes must be greater than 2.");

        Check(x => x.Prizes)
           .SetCondition(x => x.Sum(x => x.Probability) == 100)
           .WithMessage("The sum of prize probabilities should be a 100.");
    }
}