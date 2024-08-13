using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public class GetAllEndpointQueryHandler : IRequestHandler<GetAllEndpointQuery, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public GetAllEndpointQueryHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointQuery request, CancellationToken cancellationToken)
        {
            var endpoints = await _endpointRepository.GetAllEndpoints(request.RoleFilter);

            return new ApplicationResult
            {
                Success = true,
                Data = endpoints,
                Errors = null
            };
        }
    }
}
