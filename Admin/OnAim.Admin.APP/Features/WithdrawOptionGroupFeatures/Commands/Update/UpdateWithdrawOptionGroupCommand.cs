using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Update;

public record UpdateWithdrawOptionGroupCommand(UpdateWithdrawOptionGroupDto Command) : ICommand<ApplicationResult<object>>;

public sealed class UpdateWithdrawOptionGroupCommandHandler : ICommandHandler<UpdateWithdrawOptionGroupCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public UpdateWithdrawOptionGroupCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(UpdateWithdrawOptionGroupCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.UpdateWithdrawOptionGroup(request.Command);
    }
}
