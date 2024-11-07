using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypes;

public record GetPrizeTypesQuery : IQuery<ApplicationResult>;
