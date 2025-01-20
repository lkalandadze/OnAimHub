using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Abstractions;

namespace GameLib.Domain.Checkers;

public class BasePrizeChecker : Checkmate<BasePrize>
{
    public BasePrizeChecker() : base()
    {
        Check(x => x.Value)
            .GreaterThanOrEqual(0)
            .WithMessage("Value must be greater than or equal to 0.");

        Check(x => x.Probability)
            .GreaterThan(0)
            .WithMessage("The probability must be positive");

        Check(x => x.CoinId)
            .IsNotNull()
            .WithMessage("The CoinId should not be null.");
    }
}