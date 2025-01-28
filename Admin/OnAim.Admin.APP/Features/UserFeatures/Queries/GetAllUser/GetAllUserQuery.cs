using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed record GetAllUserQuery(UserFilter? UserFilter) : IQuery<ApplicationResult<PaginatedResult<UsersModel>>>;
