using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;

public record GetAllCoinQuery(BaseFilter BaseFilter) : IQuery<ApplicationResult>;
