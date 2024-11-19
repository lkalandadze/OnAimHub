using FluentValidation;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignCoinTemplatesToWithdrawOption;

public class AssignCoinTemplatesToWithdrawOptionValidator : AbstractValidator<AssignCoinTemplatesToWithdrawOption>
{
    public AssignCoinTemplatesToWithdrawOptionValidator()
    {
        RuleFor(x => x.WithdrawOptionId)
            .GreaterThan(0).WithMessage("WithdrawOptionId is required and must be greater than zero.");

        RuleFor(x => x.CoinTemplateIds)
            .NotEmpty().WithMessage("CoinTemplateIds is required and must contain at least one item.")
            .Must(ids => ids.Any()).WithMessage("CoinTemplateIds must contain at least one item.");
    }
}