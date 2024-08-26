using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public class UpdateEndpointGroupCommandHandler : IRequestHandler<UpdateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly IConfigurationRepository<Infrasturcture.Entities.EndpointGroupEndpoint> _endpointGroupEndpointRepository;
        private readonly IRepository<Infrasturcture.Entities.Endpoint> _endpointRepository;
        private readonly IValidator<UpdateEndpointGroupCommand> _validator;

        public UpdateEndpointGroupCommandHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
            IConfigurationRepository<Infrasturcture.Entities.EndpointGroupEndpoint> endpointGroupEndpointRepository,
            IRepository<Infrasturcture.Entities.Endpoint> endpointRepository,
            IValidator<UpdateEndpointGroupCommand> validator
            )
        {
            _repository = repository;
            _endpointGroupEndpointRepository = endpointGroupEndpointRepository;
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var group = await _repository
                .Query(x => x.Id == request.Id)
                .Include(g => g.EndpointGroupEndpoints)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new Exception("Endpoint Group Not Found");
            }

            if (!string.IsNullOrEmpty(request.model.Name))
            {
                bool nameExists = await _repository.Query(x => x.Name == request.model.Name && x.Id != request.Id) 
                    .AnyAsync();

                if (nameExists)
                {
                    throw new AlreadyExistsException("An Endpoint Group with this name already exists.");
                }

                group.Name = request.model.Name;
            }

            if (request.model.Description != null)
            {
                group.Description = request.model.Description;
            }

            if (request.model.IsActive.HasValue)
            {
                group.IsActive = request.model.IsActive.Value;
            }

            var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
            var newEndpointIds = request.model.EndpointIds ?? new List<int>();

            var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
            var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

            foreach (var endpointId in endpointsToAdd)
            {
                var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

                if (endpoint != null)
                {
                    var alreadyExists = group.EndpointGroupEndpoints.Any(ege => ege.EndpointId == endpointId);
                    if (!alreadyExists)
                    {

                        await _endpointGroupEndpointRepository.Store(new EndpointGroupEndpoint
                        {
                            EndpointGroupId = group.Id,
                            EndpointId = endpoint.Id,
                            Endpoint = endpoint,
                            EndpointGroup = group
                        });
                    }
                }
            }

            foreach (var endpointId in endpointsToRemove)
            {
                var endpointGroupEndpoint = await _endpointGroupEndpointRepository.Query(x => x.EndpointGroupId == group.Id && x.EndpointId == endpointId)
                    .FirstOrDefaultAsync();

                if (endpointGroupEndpoint != null)
                {
                    await _endpointGroupEndpointRepository.Remove(endpointGroupEndpoint);
                }
            }

            await _endpointGroupEndpointRepository.CommitChanges();

            return new ApplicationResult
            {
                Success = true,
                Data = request.model.Name,
                Errors = null
            };
        }
    }
}
