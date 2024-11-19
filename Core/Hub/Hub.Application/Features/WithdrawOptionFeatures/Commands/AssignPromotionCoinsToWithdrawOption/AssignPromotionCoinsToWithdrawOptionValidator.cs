using FluentValidation;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignPromotionCoinsToWithdrawOption;

public class AssignPromotionCoinsToWithdrawOptionValidator : AbstractValidator<AssignPromotionCoinsToWithdrawOption>
{
    public AssignPromotionCoinsToWithdrawOptionValidator()
    {
        RuleFor(x => x.WithdrawOptionId)
            .GreaterThan(0).WithMessage("WithdrawOptionId is required and must be greater than zero.");

        RuleFor(x => x.PromotionCoinIds)
            .NotEmpty().WithMessage("PromotionCoinIds is required and must contain at least one item.")
            .Must(ids => ids.Any()).WithMessage("PromotionCoinIds must contain at least one item.");
    }
}