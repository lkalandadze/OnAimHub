using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetById;

public sealed class GetCoinTemplateByIdQueryHandler : IQueryHandler<GetCoinTemplateByIdQuery, ApplicationResult<CoinTemplateDto>>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public GetCoinTemplateByIdQueryHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult<CoinTemplateDto>> Handle(GetCoinTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _coinTemplateService.GetCoinTemplateById(request.Id);
    }
}
