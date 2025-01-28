using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Delete;

public record DeleteWithdrawOptionCommand(List<int> Id) : ICommand<ApplicationResult<object>>;