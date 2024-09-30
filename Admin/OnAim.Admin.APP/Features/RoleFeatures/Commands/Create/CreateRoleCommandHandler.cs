using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public class CreateRoleCommandHandler : BaseCommandHandler<CreateRoleCommand, ApplicationResult>
{
    private readonly IRepository<Role> _repository;
    private readonly IRepository<EndpointGroup> _endpointGroupRepository;
    private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;

    public CreateRoleCommandHandler(
        CommandContext<CreateRoleCommand> context,
        IRepository<Role> repository,
        IRepository<EndpointGroup> EndpointGroupRepository,
        IConfigurationRepository<RoleEndpointGroup> ConfigurationRepository
        ) : base( context )
    {
        _repository = repository;
        _endpointGroupRepository = EndpointGroupRepository;
        _configurationRepository = ConfigurationRepository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existsName = _repository.Query(x => x.Name.ToLower() == request.Request.Name.ToLower()).Any();
        if (existsName)
            throw new AlreadyExistsException("Role With That Name ALready Exists");

        var role = new Role(request.Request.Name, request.Request.Description, _context.SecurityContextAccessor.UserId);

        await _repository.Store(role);
        await _repository.CommitChanges();

        foreach (var group in request.Request.EndpointGroupIds)
        {
            var epgroup = await _endpointGroupRepository.Query(x => x.Id == group).FirstOrDefaultAsync();

            if (epgroup?.IsDeleted == true)
                throw new Exception("EndpointGroup Is Disabled!");

            var roleEndpointGroup = new RoleEndpointGroup(role.Id, epgroup.Id);

            role.RoleEndpointGroups.Add(roleEndpointGroup);
            await _configurationRepository.CommitChanges();
        }

        await _configurationRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Role {role.Name} Successfully Created!",
        };
    }
}
