using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, ApplicationResult>
{
    private readonly IEndpointService _endpointService;

    public GetEndpointByIdQueryHandler(IEndpointService endpointService)
    {
        _endpointService = endpointService;
    }
    public async Task<ApplicationResult> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _endpointService.GetById(request.Id);

        return new ApplicationResult
        {
            Success = true,
            Data = result.Data
        };
    }
}
