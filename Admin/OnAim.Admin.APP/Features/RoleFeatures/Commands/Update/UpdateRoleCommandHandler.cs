﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, ApplicationResult>
{
    private readonly IRepository<Role> _repository;
    private readonly IRepository<EndpointGroup> _endpointGroupRepository;
    private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
    private readonly IValidator<UpdateRoleCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UpdateRoleCommandHandler(
        IRepository<Role> repository,
        IRepository<EndpointGroup> endpointGroupRepository,
        IConfigurationRepository<RoleEndpointGroup> configurationRepository,
        IValidator<UpdateRoleCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _endpointGroupRepository = endpointGroupRepository;
        _configurationRepository = configurationRepository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var role = await _repository.Query(x => x.Id == request.Id)
                                  .Include(r => r.RoleEndpointGroups)
                                  .ThenInclude(reg => reg.EndpointGroup)
                                  .ThenInclude(eg => eg.EndpointGroupEndpoints)
                                  .ThenInclude(ege => ege.Endpoint)
                                  .FirstOrDefaultAsync();

        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        if (!string.IsNullOrEmpty(request.Model.Name))
        {
            var super = await _repository.Query(x => x.Name == "SuperRole").FirstOrDefaultAsync();

            if (request.Model.Name == super.Name)
            {
                throw new Exception("You don't have permmission to update this role!");
            }

            bool nameExists = await _repository.Query(x => x.Name == request.Model.Name && x.Id != request.Id)
                .AnyAsync();

            if (nameExists)
            {
                throw new AlreadyExistsException("Role with this name already exists.");
            }

            role.Name = request.Model.Name;
        }

        if (!request.Model.IsActive)
        {
            role.DateUpdated = SystemDate.Now;
            role.IsActive = false;
        }

        role.Description = request.Model.Description;
        role.DateUpdated = SystemDate.Now;
        role.IsActive = request.Model.IsActive;

        if (request.Model.EndpointGroupIds != null)
        {
            var existingGroups = role.RoleEndpointGroups.Select(reg => new { reg.RoleId, reg.EndpointGroupId }).ToHashSet();

            foreach (var item in request.Model.EndpointGroupIds)
            {
                var group = await _endpointGroupRepository.Query(x => x.Id == item).FirstOrDefaultAsync();

                if (group == null)
                {
                    throw new NotFoundException("Permission Group not found");
                }

                if (!existingGroups.Contains(new { RoleId = role.Id, EndpointGroupId = group.Id }))
                {
                    var roleEndpointGroup = new RoleEndpointGroup
                    {
                        EndpointGroupId = group.Id,
                        RoleId = role.Id
                    };

                    role.RoleEndpointGroups.Add(roleEndpointGroup);
                }
            }
        }

        await _repository.CommitChanges();
        await _configurationRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Role {role.Name} Successfully Updated!",
        };
    }

}
