using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public sealed record GetAllEndpointQuery(RoleFilter RoleFilter) : IRequest<ApplicationResult>;
}
