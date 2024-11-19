using FluentValidation;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawEndpointTemplate;

public class CreateWithdrawEndpointTemplateValidator : AbstractValidator<CreateWithdrawEndpointTemplate>
{
    public CreateWithdrawEndpointTemplateValidator()
    {
        RuleFor(x => x.Endpoint)
            .NotEmpty().WithMessage("Endpoint is required.")
            .MustBeAValidUrl().WithMessage("Endpoint must be a valid URL.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MustBeAValidJson();

        RuleFor(x => x.ContentType)
            .IsInEnum().WithMessage("ContentType must be a valid value.");
    }
}