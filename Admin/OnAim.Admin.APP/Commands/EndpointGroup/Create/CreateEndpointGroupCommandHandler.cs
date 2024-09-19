using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommandHandler : ICommandHandler<CreateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly IRepository<Endpoint> _endpointRepository;
        private readonly IValidator<CreateEndpointGroupCommand> _validator;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public CreateEndpointGroupCommandHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
            IRepository<Endpoint> endpointRepository,
            IValidator<CreateEndpointGroupCommand> validator,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _repository = repository;
            _endpointRepository = endpointRepository;
            _validator = validator;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existedGroupName = await _repository.Query(x => x.Name == request.Model.Name).FirstOrDefaultAsync();

            if (existedGroupName == null)
            {
                var endpointGroup = new Infrasturcture.Entities.EndpointGroup
                {
                    Name = request.Model.Name,
                    Description = request.Model.Description,
                    IsDeleted = false,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    DateCreated = SystemDate.Now,
                    CreatedBy = _securityContextAccessor.UserId
                };

                foreach (var endpointId in request.Model.EndpointIds)
                {
                    var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

                    if (endpoint.IsDeleted)
                    {
                        throw new Exception("Permmission Is Disabled!");
                    }

                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        Endpoint = endpoint,
                        EndpointGroup = endpointGroup
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                await _repository.Store(endpointGroup);
                await _repository.CommitChanges();

                var auditLog = new AuditLog
                {
                    UserId = _securityContextAccessor.UserId,
                    Timestamp = SystemDate.Now,
                    Action = "CREATED_ENDPOINT_GROUP",
                    ObjectId = endpointGroup.Id,
                    Log = $"EndpointGroup Created successfully with ID: {endpointGroup.Id} by User ID: {_securityContextAccessor.UserId}"
                };

                await _auditLogService.LogEventAsync(auditLog);
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
}
