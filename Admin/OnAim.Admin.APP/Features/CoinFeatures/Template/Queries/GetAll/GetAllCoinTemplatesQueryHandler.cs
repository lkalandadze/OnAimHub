using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;

public sealed class GetAllCoinTemplatesQueryHandler : IQueryHandler<GetAllCoinTemplatesQuery, ApplicationResult>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public GetAllCoinTemplatesQueryHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetAllCoinTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.GetAllCoinTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
