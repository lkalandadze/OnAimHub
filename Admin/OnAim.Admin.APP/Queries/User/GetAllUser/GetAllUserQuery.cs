using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Queries.User.GetAllUser
{
    public sealed record GetAllUserQuery(UserFilter UserFilter) : IQuery<ApplicationResult>;
}
