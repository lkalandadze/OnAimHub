using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class GameConfigurationChecker : Checkmate<GameConfiguration>
{
    public GameConfigurationChecker() : base()
    {
        //Check(x => x.Name.Length)
        //    .GreaterThan(5)
        //    .WithMessage("The length of the name must be at least 5.")
        //    .LessThan(30)
        //    .WithMessage("The length of the name must be more than 30.");

        //Check(x => x.Value)
        //   .GreaterThan(0)
        //   .WithMessage("The value must be positive");
    }
}