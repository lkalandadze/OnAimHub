using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetById;

public sealed class GetGameConfigurationTemplateByIdQueryHandler : IQueryHandler<GetGameConfigurationTemplateByIdQuery, ApplicationResult<GameConfigurationTemplate>>
{
    private readonly IGameTemplateService _gameTemplateService;

    public GetGameConfigurationTemplateByIdQueryHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult<GameConfigurationTemplate>> Handle(GetGameConfigurationTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _gameTemplateService.GetGameConfigurationTemplateById(request.Id);
    }
}
