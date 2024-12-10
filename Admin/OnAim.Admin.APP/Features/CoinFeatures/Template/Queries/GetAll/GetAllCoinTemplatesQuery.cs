using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;

public record GetAllCoinTemplatesQuery() : IQuery<ApplicationResult>;
