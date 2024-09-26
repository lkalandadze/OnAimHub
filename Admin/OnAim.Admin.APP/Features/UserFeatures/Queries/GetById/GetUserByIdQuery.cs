using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;

public sealed record GetUserByIdQuery(int Id) : IQuery<ApplicationResult>;
