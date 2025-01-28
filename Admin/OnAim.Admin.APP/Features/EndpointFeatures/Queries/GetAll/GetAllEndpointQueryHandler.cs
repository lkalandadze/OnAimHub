using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;

public class GetAllEndpointQueryHandler : IQueryHandler<GetAllEndpointQuery, ApplicationResult<PaginatedResult<EndpointResponseModel>>>
{
    private readonly IEndpointService _endpointService;

    public GetAllEndpointQueryHandler(IEndpointService endpointService)
    {
        _endpointService = endpointService;
    }
    public async Task<ApplicationResult<PaginatedResult<EndpointResponseModel>>> Handle(GetAllEndpointQuery request, CancellationToken cancellationToken)
    {
        return await _endpointService.GetAll(request.Filter);
    }
}
