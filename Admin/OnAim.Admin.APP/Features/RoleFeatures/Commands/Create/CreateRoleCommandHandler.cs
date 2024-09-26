using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, ApplicationResult>
{
    private readonly IRepository<Role> _repository;
    private readonly IRepository<EndpointGroup> _endpointGroupRepository;
    private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
    private readonly IValidator<CreateRoleCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CreateRoleCommandHandler(
        IRepository<Role> repository,
        IRepository<EndpointGroup> EndpointGroupRepository,
        IConfigurationRepository<RoleEndpointGroup> ConfigurationRepository,
        IValidator<CreateRoleCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _endpointGroupRepository = EndpointGroupRepository;
        _configurationRepository = ConfigurationRepository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var existsName = _repository.Query(x => x.Name.ToLower() == request.Request.Name.ToLower()).Any();
        if (existsName)
        {
            throw new AlreadyExistsException("Role With That Name ALready Exists");
        }
        var role = new Role
        {
            Name = request.Request.Name,
            Description = request.Request.Description,
            DateCreated = SystemDate.Now,
            IsActive = true,
            RoleEndpointGroups = new List<RoleEndpointGroup>(),
            CreatedBy = _securityContextAccessor.UserId,
        };
        await _repository.Store(role);
        await _repository.CommitChanges();

        foreach (var group in request.Request.EndpointGroupIds)
        {
            var epgroup = await _endpointGroupRepository.Query(x => x.Id == group).FirstOrDefaultAsync();

            if (epgroup.IsDeleted)
            {
                throw new Exception("EndpointGroup Is Disabled!");
            }

            var roleEndpointGroup = new RoleEndpointGroup
            {
                EndpointGroupId = epgroup.Id,
                RoleId = role.Id
            };

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
