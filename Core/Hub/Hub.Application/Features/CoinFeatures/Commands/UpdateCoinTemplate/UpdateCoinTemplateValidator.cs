using FluentValidation;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.CoinFeatures.Commands.UpdateCoinTemplate;

public class UpdateCoinTemplateValidator : AbstractValidator<UpdateCoinTemplate>
{
    public UpdateCoinTemplateValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MustBeAValidUrl().WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.CoinType)
            .IsInEnum().WithMessage("CoinType must be a valid value.");
    }
}