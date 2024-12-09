using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetById;

public sealed class GetGameConfigurationTemplateByIdQueryHandler : IQueryHandler<GetGameConfigurationTemplateByIdQuery, ApplicationResult>
{
    private readonly IGameTemplateService _gameTemplateService;

    public GetGameConfigurationTemplateByIdQueryHandler(IGameTemplateService gameTemplateService)
    {
        _gameTemplateService = gameTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetGameConfigurationTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _gameTemplateService.GetGameConfigurationTemplateById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
