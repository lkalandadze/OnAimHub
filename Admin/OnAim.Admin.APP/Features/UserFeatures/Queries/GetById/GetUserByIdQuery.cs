using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;

public sealed record GetUserByIdQuery(int Id) : IQuery<ApplicationResult>;
