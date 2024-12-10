using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetById;

public sealed class GetCoinTemplateByIdQueryHandler : IQueryHandler<GetCoinTemplateByIdQuery, ApplicationResult>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public GetCoinTemplateByIdQueryHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetCoinTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _coinTemplateService.GetCoinTemplateById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
