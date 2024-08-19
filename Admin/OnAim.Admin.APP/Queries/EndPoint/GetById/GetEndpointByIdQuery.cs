using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public record GetEndpointByIdQuery(int Id) : IRequest<ApplicationResult>;
}
