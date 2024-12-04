using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;
namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;

public class GetEndpointGroupByIdQueryHandler : IQueryHandler<GetEndpointGroupByIdQuery, ApplicationResult>
{
    private readonly IEndpointGroupService _endpointGroupService;

    public GetEndpointGroupByIdQueryHandler(IEndpointGroupService endpointGroupService)
    {
        _endpointGroupService = endpointGroupService;
    }
    public async Task<ApplicationResult> Handle(GetEndpointGroupByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await _endpointGroupService.GetById(request.Id);

        return new ApplicationResult
        {
            Success = true,
            Data = result,
        };
    }
}
