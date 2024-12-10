using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;

public sealed class CreateCoinTemplateCommandHandler : ICommandHandler<CreateCoinTemplateCommand, CoinTemplate>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public CreateCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<CoinTemplate> Handle(CreateCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _coinTemplateService.CreateCoinTemplate(request.create);
    }
}
