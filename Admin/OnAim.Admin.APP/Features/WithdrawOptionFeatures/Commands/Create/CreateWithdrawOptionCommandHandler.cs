using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Create;

public sealed class CreateWithdrawOptionCommandHandler : ICommandHandler<CreateWithdrawOptionCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(CreateWithdrawOptionCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.CreateWithdrawOption(request.Command);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
