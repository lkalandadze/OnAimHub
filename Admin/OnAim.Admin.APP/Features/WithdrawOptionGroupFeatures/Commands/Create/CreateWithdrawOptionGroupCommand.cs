using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Create;

public record CreateWithdrawOptionGroupCommand(CreateWithdrawOptionGroupDto Command) : ICommand<ApplicationResult>;

public sealed class CreateWithdrawOptionGroupCommandHandler : ICommandHandler<CreateWithdrawOptionGroupCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionGroupCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(CreateWithdrawOptionGroupCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.CreateWithdrawOptionGroup(request.Command);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
