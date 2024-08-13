using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public class UpdateEndpointGroupCommandHandler : IRequestHandler<UpdateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public UpdateEndpointGroupCommandHandler(
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository
            )
        {
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _endpointGroupRepository.GetByIdAsync(request.Id);

            if (group == null)
            {
                throw new Exception("Permission Group Not Found");
            }

            var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
            var newEndpointIds = request.model.EndpointIds ?? new List<string>();

            var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
            var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

            foreach (var endpointId in endpointsToAdd)
            {
                var endpoint = await _endpointRepository.GetEndpointById(endpointId);

                var ep = new Endpoint
                {
                    Id = endpoint.Id,
                    Name = endpoint.Name,
                    Path = endpoint.Path,
                    Description = endpoint.Description,
                    IsActive = endpoint.IsActive,
                    IsEnabled = endpoint.IsEnabled,
                    UserId = endpoint.UserId,
                    Type = endpoint.Type,
                    DateCreated = endpoint.DateCreated,
                };
                if (endpoint != null)
                {
                    await _endpointGroupRepository.AddEndpoint(group, ep);
                }
            }

            foreach (var endpointId in endpointsToRemove)
            {
                var endpoint = await _endpointRepository.GetEndpointById(endpointId);
                var ep = new Endpoint
                {
                    Id = endpoint.Id,
                    Name = endpoint.Name,
                    Path = endpoint.Path,
                    Description = endpoint.Description,
                    IsActive = endpoint.IsActive,
                    IsEnabled = endpoint.IsEnabled,
                    UserId = endpoint.UserId,
                    Type = endpoint.Type,
                    DateCreated = endpoint.DateCreated,
                };

                if (endpoint != null)
                {
                    await _endpointGroupRepository.RemoveEndpoint(group, ep);
                }
            }

            await _endpointGroupRepository.SaveChangesAsync();

            return new ApplicationResult
            {
                Success = true,
                Data = group,
                Errors = null
            };
        }
    }
}
