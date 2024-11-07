using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypeById;

public record GetPrizeTypeByIdQuery(int Id) : IQuery<ApplicationResult>;
