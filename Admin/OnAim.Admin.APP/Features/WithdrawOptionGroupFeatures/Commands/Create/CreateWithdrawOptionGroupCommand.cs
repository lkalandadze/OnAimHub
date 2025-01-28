using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Create;

public record CreateWithdrawOptionGroupCommand(CreateWithdrawOptionGroupDto Command) : ICommand<ApplicationResult<object>>;

public sealed class CreateWithdrawOptionGroupCommandHandler : ICommandHandler<CreateWithdrawOptionGroupCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionGroupCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(CreateWithdrawOptionGroupCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.CreateWithdrawOptionGroup(request.Command);
    }
}
