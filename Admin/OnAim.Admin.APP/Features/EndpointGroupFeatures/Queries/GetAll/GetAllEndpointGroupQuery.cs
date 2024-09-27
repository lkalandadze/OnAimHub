using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;

public record GetAllEndpointGroupQuery(EndpointGroupFilter Filter) : IQuery<ApplicationResult>;
