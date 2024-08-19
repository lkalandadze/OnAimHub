using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetAllUser
{
    public sealed record GetAllUserQuery(UserFilter UserFilter) : IRequest<ApplicationResult>;
}
