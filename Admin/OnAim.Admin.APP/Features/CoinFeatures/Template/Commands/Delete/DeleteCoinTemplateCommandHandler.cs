using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;

public sealed class DeleteCoinTemplateCommandHandler : ICommandHandler<DeleteCoinTemplateCommand, ApplicationResult<bool>>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public DeleteCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.DeleteCoinTemplate(request.Id);

        return new ApplicationResult<bool> { Data = result.Data, Success = result.Success };
    }
}
