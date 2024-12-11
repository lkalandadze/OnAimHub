using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class GameConfigurationChecker : Checkmate<GameConfiguration>
{
    public GameConfigurationChecker() : base()
    {
        //Check(x => x.Name.Length)
        //    .GreaterThan(10)
        //    .WithMessage("The length of the configuration name must be at least 10.")
        //    .LessThan(30)
        //    .WithMessage("The length of the configuration name must be less than 30.");

        //Check(x => x.Value)
        //   .GreaterThan(0)
        //   .WithMessage("The configuration value must be positive");
    }
}