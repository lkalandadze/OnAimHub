using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Update;

public sealed class UpdateCoinTemplateCommandHandler : ICommandHandler<UpdateCoinTemplateCommand, ApplicationResult>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public UpdateCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult> Handle(UpdateCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.UpdateCoinTemplate(request.update);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
