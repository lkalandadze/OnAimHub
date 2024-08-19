using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetById
{
    public record GetEndpointGroupByIdQuery(int Id) : IRequest<ApplicationResult>;
}
