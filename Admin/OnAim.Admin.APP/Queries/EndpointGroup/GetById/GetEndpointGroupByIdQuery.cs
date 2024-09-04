using MediatR;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetById
{
    public record GetEndpointGroupByIdQuery(int Id) : IQuery<ApplicationResult>;
}
