using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;

public sealed record GetAllEndpointQuery(EndpointFilter Filter) : IQuery<ApplicationResult>;
