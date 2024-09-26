using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed record GetAllUserQuery(UserFilter? UserFilter) : IQuery<ApplicationResult>;
