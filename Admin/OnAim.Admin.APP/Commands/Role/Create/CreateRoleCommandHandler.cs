using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _endpointGroupRepository;
        private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
        private readonly IValidator<CreateRoleCommand> _validator;

        public CreateRoleCommandHandler(
            IRepository<Infrasturcture.Entities.Role> repository,
            IRepository<Infrasturcture.Entities.EndpointGroup> EndpointGroupRepository,
            IConfigurationRepository<RoleEndpointGroup> ConfigurationRepository,
            IValidator<CreateRoleCommand> validator
            )
        {
            _repository = repository;
            _endpointGroupRepository = EndpointGroupRepository;
            _configurationRepository = ConfigurationRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existsName = _repository.Query(x => x.Name == request.request.Name).Any();
            if (existsName)
            {
                throw new Exception("Role With That Name ALready Exists");
            }
            var role = new Infrasturcture.Entities.Role
            {
                Name = request.request.Name,
                Description = request.request.Description,
                DateCreated = SystemDate.Now,
                IsActive = true,
                RoleEndpointGroups = new List<RoleEndpointGroup>(),
                //UserId = request.ParentUserId,
            };
            await _repository.Store(role);
            await _repository.CommitChanges();

            foreach (var group in request.request.EndpointGroupIds)
            {
                var epgroup = await _endpointGroupRepository.Query(x => x.Id == group).FirstOrDefaultAsync();

                if (!epgroup.IsEnabled)
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
                Data = role,
                Errors = null
            };
        }
    }
}
