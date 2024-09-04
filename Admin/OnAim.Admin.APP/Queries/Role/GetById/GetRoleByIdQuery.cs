using MediatR;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetById
{
    public record GetRoleByIdQuery(int Id) : IQuery<ApplicationResult>;
}
