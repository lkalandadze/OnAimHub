using FluentValidation;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;

public class RevokePlayerBanCommandValidator : AbstractValidator<RevokePlayerBanCommand>
{
    public RevokePlayerBanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
