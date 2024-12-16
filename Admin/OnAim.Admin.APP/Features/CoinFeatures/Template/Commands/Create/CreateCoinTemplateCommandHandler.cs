using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;

public sealed class CreateCoinTemplateCommandHandler : ICommandHandler<CreateCoinTemplateCommand, CoinTemplateListDto>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public CreateCoinTemplateCommandHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<CoinTemplateListDto> Handle(CreateCoinTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _coinTemplateService.CreateCoinTemplate(request.create);
    }
}
