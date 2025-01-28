using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.AddBalanceToPlayer;

public sealed class AddBalanceToPlayerCommandHandler : ICommandHandler<AddBalanceToPlayerCommand, ApplicationResult<bool>>
{
    private readonly IPlayerService _playerService;

    public AddBalanceToPlayerCommandHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult<bool>> Handle(AddBalanceToPlayerCommand request, CancellationToken cancellationToken)
    {
        return await _playerService.AddBalanceToPlayer(request.Command);
    }
}
