using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;

public sealed class GetAllGameConfigurationTemplatesQueryHandler : IQueryHandler<GetAllGameConfigurationTemplatesQuery, ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>>
{
    private readonly IGameTemplateService _gameTemplateService;

    public GetAllGameConfigurationTemplatesQueryHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>> Handle(GetAllGameConfigurationTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _gameTemplateService.GetAllGameConfigurationTemplates(request.Filter);
    }
}
