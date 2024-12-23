using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Abstractions;

namespace GameLib.Domain.Checkers;

public class BasePrizeChecker : Checkmate<BasePrize>
{
    public BasePrizeChecker() : base()
    {
        Check(x => x.Value)
            .GreaterThan(0)
            .WithMessage("The value of price must be positive.");

        Check(x => x.Probability)
            .GreaterThan(0)
            .WithMessage("The Probability property must be positive.");

        Check(x => x.CoinId)
            .IsNotNull()
            .WithMessage("The CoinId should not be null.");
    }
}