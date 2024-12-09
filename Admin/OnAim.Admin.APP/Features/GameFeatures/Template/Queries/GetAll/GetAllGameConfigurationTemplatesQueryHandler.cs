using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;

public sealed class GetAllGameConfigurationTemplatesQueryHandler : IQueryHandler<GetAllGameConfigurationTemplatesQuery, ApplicationResult>
{
    private readonly IGameTemplateService _gameTemplateService;

    public GetAllGameConfigurationTemplatesQueryHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetAllGameConfigurationTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _gameTemplateService.GetAllGameConfigurationTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
