using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Update;

public sealed class UpdateCoinTemplateCommandHandler : ICommandHandler<UpdateCoinTemplateCommand, ApplicationResult<bool>>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public UpdateCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(UpdateCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.UpdateCoinTemplate(request.update);

        return new ApplicationResult<bool> { Data = result.Data, Success = result.Success };
    }
}
