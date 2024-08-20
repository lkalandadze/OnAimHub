using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetById
{
    public record GetRoleByIdQuery(int Id) : IRequest<ApplicationResult>;
}
