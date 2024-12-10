using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class PriceChecker : Checkmate<Price>
{
    public PriceChecker()
    {
        Check(x => x.Value)
            .GreaterThan(0)
            .WithMessage("The value of price must be positive.");

        Check(x => x.Multiplier)
            .GreaterThan(0)
            .WithMessage("The multiplier of price must be positive.");
    }
}