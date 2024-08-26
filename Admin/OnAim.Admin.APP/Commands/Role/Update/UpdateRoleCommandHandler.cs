using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using static OnAim.Admin.APP.Exceptions.Exceptions;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _endpointGroupRepository;
        private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
        private readonly IValidator<UpdateRoleCommand> _validator;

        public UpdateRoleCommandHandler(
            IRepository<Infrasturcture.Entities.Role> repository,
            IRepository<Infrasturcture.Entities.EndpointGroup> endpointGroupRepository,
            IConfigurationRepository<RoleEndpointGroup> configurationRepository,
            IValidator<UpdateRoleCommand> validator
            )
        {
            _repository = repository;
            _endpointGroupRepository = endpointGroupRepository;
            _configurationRepository = configurationRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _repository.Query(x => x.Id == request.Id)
                                      .Include(r => r.RoleEndpointGroups)
                                      .ThenInclude(reg => reg.EndpointGroup)
                                      .ThenInclude(eg => eg.EndpointGroupEndpoints)
                                      .ThenInclude(ege => ege.Endpoint)
                                      .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new RoleNotFoundException("Role not found");
            }

            if (!request.Model.IsActive)
            {
                role.DateUpdated = SystemDate.Now;
                role.IsActive = false;
            }

            role.Name = request.Model.Name;
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
                        throw new Exception("Permission Group Not Found");
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
                Data = role,
                Errors = null
            };
        }

    }
}
