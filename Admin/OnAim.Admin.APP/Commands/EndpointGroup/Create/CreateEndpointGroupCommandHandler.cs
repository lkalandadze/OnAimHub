using MediatR;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommandHandler : IRequestHandler<CreateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public CreateEndpointGroupCommandHandler(
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository
            )
        {
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var endpointGroup = new Infrasturcture.Entities.EndpointGroup
            {
                Name = request.Name,
                Description = request.Description,
                IsEnabled = true,
                IsActive = true,
                EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                UserId = request.UserId,
                DateCreated = SystemDate.Now,
            };

            foreach (var endpointId in request.EndpointIds)
            {
                var endpoint = _endpointRepository.GetEndpointById(endpointId).Result;
                if (!endpoint.IsEnabled)
                {
                    throw new Exception("Endpoint Is Disabled!");
                }

                var endpointGroupEndpoint = new EndpointGroupEndpoint
                {
                    Endpoint = endpoint,
                    EndpointGroup = endpointGroup
                };

                endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
            }

            await _endpointGroupRepository.AddAsync(endpointGroup);
            await _endpointGroupRepository.SaveChangesAsync();

            return new ApplicationResult
            {
                Success = true,
                Data = endpointGroup.Name,
                Errors = null
            };
        }
    }
}
