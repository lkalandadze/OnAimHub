using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommandHandler : IRequestHandler<CreateEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public CreateEndpointCommandHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
        {
            var result = await _endpointRepository.CreateEndpointAsync(request.Name, request.Description, request.Type, request.UserId);

            if (result == null) { }

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };

        }
    }
}
