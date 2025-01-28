using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.AddBalanceToPlayer;

public record AddBalanceToPlayerCommand(AddBalanceDto Command) : ICommand<ApplicationResult<bool>>;
