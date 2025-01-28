using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;

public record GetAllGameConfigurationTemplatesQuery(GameTemplateFilter Filter) : IQuery<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>>;
