using Checkmate;
using CheckmateValidations;
using TestWheel.Domain.Entities;

namespace TestWheel.Domain.Checkers;

public class TestWheelPrizeChecker : Checkmate<TestWheelPrize>
{
    public TestWheelPrizeChecker() : base()
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

        Check(x => x.Index)
           .GreaterThan(0)
           .WithMessage("The index must be positive");
    }
}