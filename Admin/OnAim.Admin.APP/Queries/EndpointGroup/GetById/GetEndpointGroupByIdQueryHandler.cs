using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetById
{
    public class GetEndpointGroupByIdQueryHandler : IRequestHandler<GetEndpointGroupByIdQuery, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;

        public GetEndpointGroupByIdQueryHandler(IEndpointGroupRepository endpointGroupRepository)
        {
            _endpointGroupRepository = endpointGroupRepository;
        }
        public async Task<ApplicationResult> Handle(GetEndpointGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _endpointGroupRepository.GetByIdAsync(request.Id);

            return new ApplicationResult
            {
                Success = true,
                Data = res,
                Errors = null
            };
        }
    }
}
