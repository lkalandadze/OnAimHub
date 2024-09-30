using FluentValidation;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands
{
    public class BanPlayerCommandValidator : AbstractValidator<BanPlayerCommand>    
    {
        public BanPlayerCommandValidator()
        {
            RuleFor(x => x.PlayerId).NotEmpty().NotNull();
        }
    }
}
