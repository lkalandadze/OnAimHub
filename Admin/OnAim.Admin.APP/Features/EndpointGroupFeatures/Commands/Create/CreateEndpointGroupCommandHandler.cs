using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;

public class CreateEndpointGroupCommandHandler : BaseCommandHandler<CreateEndpointGroupCommand, ApplicationResult>
{
    private readonly IRepository<EndpointGroup> _repository;
    private readonly IRepository<Endpoint> _endpointRepository;

    public CreateEndpointGroupCommandHandler(
        CommandContext<CreateEndpointGroupCommand> context,
        IRepository<EndpointGroup> repository,
        IRepository<Endpoint> endpointRepository
        ) : base( context )
    {
        _repository = repository;
        _endpointRepository = endpointRepository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existedGroupName = await _repository.Query(x => x.Name == request.Model.Name).FirstOrDefaultAsync();

        if (existedGroupName == null)
        {
            var endpointGroup = EndpointGroup.Create(request.Model.Name, request.Model.Description, _context.SecurityContextAccessor.UserId, new List<EndpointGroupEndpoint>());

            foreach (var endpointId in request.Model.EndpointIds)
            {
                var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

                if (endpoint?.IsDeleted == true)
                {
                    throw new Exception("Permmission Is Disabled!");
                }

                var endpointGroupEndpoint = new EndpointGroupEndpoint(endpointGroup.Id, endpoint.Id);

                endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
            }

            await _repository.Store(endpointGroup);
            await _repository.CommitChanges();
        }
        else
        {
            throw new Exception("Permmission Group with that name already exists!");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission Group {request.Model.Name} Successfully Created",
        };
    }
}
