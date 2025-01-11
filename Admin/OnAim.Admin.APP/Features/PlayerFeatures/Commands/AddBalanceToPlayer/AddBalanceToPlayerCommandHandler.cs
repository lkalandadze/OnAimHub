using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.AddBalanceToPlayer;

public sealed class AddBalanceToPlayerCommandHandler : ICommandHandler<AddBalanceToPlayerCommand, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public AddBalanceToPlayerCommandHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult> Handle(AddBalanceToPlayerCommand request, CancellationToken cancellationToken)
    {
        var res = await _playerService.AddBalanceToPlayer(request.Command);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
