using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public record GetAllEndpointGroupQuery(EndpointGroupFilter Filter) : IQuery<ApplicationResult>;
}
