using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Enable
{
    public class EnableEndpointCommandHandler : IRequestHandler<EnableEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public EnableEndpointCommandHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(EnableEndpointCommand request, CancellationToken cancellationToken)
        {
            var res = await _endpointRepository.EnableEndpointAsync(request.EndpointId);

            return new ApplicationResult
            {
                Success = true,
                Data = res,
                Errors = null
            };
        }
    }
}
