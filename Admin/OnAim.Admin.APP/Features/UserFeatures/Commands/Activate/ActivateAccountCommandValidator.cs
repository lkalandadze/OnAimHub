using FluentValidation;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.Activate
{
    public class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
    {
        public ActivateAccountCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
