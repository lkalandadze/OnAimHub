using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public class UpdateEndpointGroupCommandHandler : BaseCommandHandler<UpdateEndpointGroupCommand, ApplicationResult>
{
    private readonly IRepository<EndpointGroup> _repository;
    private readonly IConfigurationRepository<EndpointGroupEndpoint> _endpointGroupEndpointRepository;
    private readonly IRepository<Endpoint> _endpointRepository;

    public UpdateEndpointGroupCommandHandler(
        CommandContext<UpdateEndpointGroupCommand> context,
        IRepository<EndpointGroup> repository,
        IConfigurationRepository<EndpointGroupEndpoint> endpointGroupEndpointRepository,
        IRepository<Endpoint> endpointRepository
        ) : base( context )
    {
        _repository = repository;
        _endpointGroupEndpointRepository = endpointGroupEndpointRepository;
        _endpointRepository = endpointRepository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var group = await _repository
            .Query(x => x.Id == request.Id)
            .Include(g => g.EndpointGroupEndpoints)
            .FirstOrDefaultAsync();

        if (group == null)
            throw new BadRequestException("Permmission Group Not Found");

        if (!string.IsNullOrEmpty(request.model.Name))
        {
            var super = await _repository.Query(x => x.Name == "SuperGroup").FirstOrDefaultAsync();

            if (request.model.Name == super.Name)
                throw new BadRequestException("You don't have permmission to update this group!");

            bool nameExists = await _repository.Query(x => x.Name == request.model.Name.ToLower() && x.Id != request.Id)
                .AnyAsync();

            if (nameExists)
                throw new BadRequestException("Permmission Group with this name already exists.");

            group.Name = request.model.Name.ToLower();
        }

        if (request.model.Description != null)
            group.Description = request.model.Description;

        if (request.model.IsActive.HasValue)
            group.IsActive = request.model.IsActive.Value;

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
                    await _endpointGroupEndpointRepository.Store(new EndpointGroupEndpoint(group.Id, endpoint.Id));             
                }
            }
        }

        foreach (var endpointId in endpointsToRemove)
        {
            var endpointGroupEndpoint = await _endpointGroupEndpointRepository.Query(x => x.EndpointGroupId == group.Id && x.EndpointId == endpointId)
                .FirstOrDefaultAsync();

            if (endpointGroupEndpoint != null)
                await _endpointGroupEndpointRepository.Remove(endpointGroupEndpoint);
        }

        group.DateUpdated = SystemDate.Now;

        await _endpointGroupEndpointRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission Group {request.model.Name} Successfully Updated",
        };
    }
}
