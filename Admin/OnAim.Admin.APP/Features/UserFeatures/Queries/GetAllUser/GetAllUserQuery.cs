using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed record GetAllUserQuery(UserFilter? UserFilter) : IQuery<ApplicationResult>;
