using FluentValidation;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;

public class UpdatePlayerBanCommandValidator : AbstractValidator<UpdatePlayerBanCommand>
{
    public UpdatePlayerBanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
