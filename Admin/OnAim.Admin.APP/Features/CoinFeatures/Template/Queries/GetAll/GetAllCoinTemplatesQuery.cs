using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;

public record GetAllCoinTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<CoinTemplateListDto>>>;
