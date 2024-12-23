using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class GameConfigurationChecker : Checkmate<GameConfiguration>
{
    public GameConfigurationChecker() : base()
    {
        Check(x => x.Name)
            .IsNotNull()
            .WithMessage("The name should not be null.");

        Check(x => x.Value)
           .GreaterThan(0)
           .WithMessage("The configuration value must be positive.");

        Check(x => x.PromotionId)
            .GreaterThan(0)
            .WithMessage("The promotion id is required.");

        Check(x => x.CorrelationId)
            .IsNotNull()
            .WithMessage("The correlation id should not be null.");
    }
}