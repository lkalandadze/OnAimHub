using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;

public record GetAllCoinTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult>;
