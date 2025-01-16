using CheckmateValidations;
using Wheel.Domain.Entities;
using Checkmate;

namespace Wheel.Domain.Checkers;

public class WheelPrizeChecker : Checkmate<WheelPrize>
{
    public WheelPrizeChecker() : base()
    {
        Check(x => x.Name)
          .IsNotNull()
          .WithMessage("The name should not be null.");

        //Check(x => x.Value)
        //   .GreaterThan(0)
        //   .WithMessage("The value must be positive");

        Check(x => x.Probability)
           .GreaterThan(0)
           .WithMessage("The probability must be positive");
    }
}