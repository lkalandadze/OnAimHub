using FluentValidation;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;

public class UpdateWithdrawOptionValidator : AbstractValidator<UpdateWithdrawOption>
{
    public UpdateWithdrawOptionValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MustBeAValidUrl().WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x)
            .Must(x => x.PromotionCoinIds?.Any() == true || x.CoinTemplateIds?.Any() == true)
            .WithMessage("At least one of PromotionCoinIds or CoinTemplateIds must be provided.");

        RuleFor(x => x)
            .Must(x => (!string.IsNullOrWhiteSpace(x.Endpoint) && !string.IsNullOrWhiteSpace(x.EndpointContent)) || x.TemplateId.HasValue)
            .WithMessage("Either both Endpoint and EndpointContent must be provided, or FromTemplateId must be specified.");

        RuleFor(x => x.Endpoint)
            .MustBeAValidUrl().When(x => !string.IsNullOrEmpty(x.Endpoint))
            .WithMessage("Endpoint must be a valid URL.");

        RuleFor(x => x.EndpointContent)
            .MustBeAValidJson().When(x => !string.IsNullOrEmpty(x.EndpointContent))
            .WithMessage("EndpointContent must be a valid JSON string.");
    }
}