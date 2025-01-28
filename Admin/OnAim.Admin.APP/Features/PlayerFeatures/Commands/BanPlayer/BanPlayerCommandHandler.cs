using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

public class BanPlayerCommandHandler : ICommandHandler<BanPlayerCommand, ApplicationResult<bool>>
{
    private readonly IPlayerService _playerService;
    private readonly IValidator<BanPlayerCommand> _validator;

    public BanPlayerCommandHandler(IPlayerService playerService, IValidator<BanPlayerCommand> validator)
    {
        _playerService = playerService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(BanPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _playerService.BanPlayer(request.PlayerId, request.ExpireDate, request.IsPermanent, request.Description);
    }
}
