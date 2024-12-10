using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;

public sealed class DeleteCoinTemplateCommandHandler : ICommandHandler<DeleteCoinTemplateCommand, ApplicationResult>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public DeleteCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult> Handle(DeleteCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.DeleteCoinTemplate(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
