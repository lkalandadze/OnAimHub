using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Update;

public record UpdateWithdrawOptionCommand(UpdateWithdrawOptionDto Command) : ICommand<ApplicationResult>;
