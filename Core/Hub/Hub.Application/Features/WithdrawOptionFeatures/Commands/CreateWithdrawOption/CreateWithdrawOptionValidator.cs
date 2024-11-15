using FluentValidation;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

public class CreateWithdrawOptionValidator : AbstractValidator<CreateWithdrawOption>
{
    public CreateWithdrawOptionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MustBeAValidUrl().WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.PromotionCoinIds)
            .NotEmpty().WithMessage("PromotionCoinIds is required and must contain at least one item.")
            .Must(ids => ids.Any()).WithMessage("PromotionCoinIds must contain at least one item.");

        RuleFor(x => x.Endpoint)
            .Empty().Null()
            .MustBeAValidUrl().When(x => !string.IsNullOrEmpty(x.Endpoint))
            .WithMessage("Endpoint must be a valid URL.");

        RuleFor(x => x.EndpointContent)
            .Empty().Null()
            .MustBeAValidJson().When(x => !string.IsNullOrEmpty(x.EndpointContent))
            .WithMessage("EndpointContent must be a valid JSON string.");

        RuleFor(x => x.TemplateId)
            .Null()
            .GreaterThan(0).When(x => x.TemplateId.HasValue)
            .WithMessage("TemplateId must be greater than zero if provided.");
    }
}