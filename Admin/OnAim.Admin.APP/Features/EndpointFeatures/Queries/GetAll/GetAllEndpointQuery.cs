using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;

public sealed record GetAllEndpointQuery(EndpointFilter Filter) : IQuery<ApplicationResult<PaginatedResult<EndpointResponseModel>>>;
