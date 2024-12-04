using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;

public class GetAllEndpointQueryHandler : IQueryHandler<GetAllEndpointQuery, ApplicationResult>
{
    private readonly IEndpointService _endpointService;

    public GetAllEndpointQueryHandler(IEndpointService endpointService)
    {
        _endpointService = endpointService;
    }
    public async Task<ApplicationResult> Handle(GetAllEndpointQuery request, CancellationToken cancellationToken)
    {
        var result = await _endpointService.GetAll(request.Filter);

        return new ApplicationResult
        {
            Success = true,
            Data = result
        };
    }
}
