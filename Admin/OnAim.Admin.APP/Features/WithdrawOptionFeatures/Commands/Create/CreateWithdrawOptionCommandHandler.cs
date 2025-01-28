using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Create;

public sealed class CreateWithdrawOptionCommandHandler : ICommandHandler<CreateWithdrawOptionCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(CreateWithdrawOptionCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.CreateWithdrawOption(request.Command);
    }
}
