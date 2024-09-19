using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public record GetAllEndpointGroupQuery(EndpointGroupFilter Filter) : IQuery<ApplicationResult>;
}
