using FluentValidation;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.CoinFeatures.Commands.CreatePromotionCoin;

public class CreatePromotionCoinValidator : AbstractValidator<CreatePromotionCoin>
{
    public CreatePromotionCoinValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.IconUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MustBeAValidUrl().WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.PromotionId)
            .GreaterThan(0).WithMessage("PromotionId is required.");

        RuleFor(x => x.CoinType)
            .IsInEnum().WithMessage("CoinType must be a valid value.");

        RuleFor(x => x.WithdrawOptionIds)
            .Null().Empty();
    }
}