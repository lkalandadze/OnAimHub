using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, ApplicationResult<EndpointResponseModel>>
{
    private readonly IEndpointService _endpointService;

    public GetEndpointByIdQueryHandler(IEndpointService endpointService)
    {
        _endpointService = endpointService;
    }
    public async Task<ApplicationResult<EndpointResponseModel>> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        return await _endpointService.GetById(request.Id);
    }
}
