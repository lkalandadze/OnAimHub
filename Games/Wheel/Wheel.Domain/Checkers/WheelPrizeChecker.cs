using CheckmateValidations;
using Wheel.Domain.Entities;
using Checkmate;

namespace Wheel.Domain.Checkers;

public class WheelPrizeChecker : Checkmate<WheelPrize>
{
    public WheelPrizeChecker() : base()
    {
        Check(x => x.Name.Length)
            .GreaterThan(3)
            .WithMessage("The length of the name must be at least 3.")
            .LessThan(35)
            .WithMessage("The length of the name must be more than 35.");

        Check(x => x.Value)
           .GreaterThan(0)
           .WithMessage("The value must be positive");

        Check(x => x.Probability)
           .GreaterThan(0)
           .WithMessage("The probability must be positive");

        Check(x => x.WheelIndex)
           .GreaterThan(0)
           .WithMessage("The wheel index must be positive");
    }
}