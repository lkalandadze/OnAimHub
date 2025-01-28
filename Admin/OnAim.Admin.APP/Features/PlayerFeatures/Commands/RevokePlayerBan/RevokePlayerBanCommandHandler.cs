using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;

public class RevokePlayerBanCommandHandler : ICommandHandler<RevokePlayerBanCommand, ApplicationResult<bool>>
{
    private readonly IPlayerService _playerService;
    private readonly IValidator<RevokePlayerBanCommand> _validator;

    public RevokePlayerBanCommandHandler(IPlayerService playerService, IValidator<RevokePlayerBanCommand> validator) 
    {
        _playerService = playerService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(RevokePlayerBanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _playerService.RevokeBan(request.Id);
    }
}
