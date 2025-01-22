using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;

public sealed class CreateCoinTemplateCommandHandler : ICommandHandler<CreateCoinTemplateCommand, bool>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public CreateCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<bool> Handle(CreateCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _coinTemplateService.CreateCoinTemplate(request.create);
    }
}
