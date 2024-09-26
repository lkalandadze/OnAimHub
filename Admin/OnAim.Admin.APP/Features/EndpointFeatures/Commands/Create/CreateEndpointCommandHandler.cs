using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommandHandler : ICommandHandler<CreateEndpointCommand, ApplicationResult>
{
    private readonly IValidator<CreateEndpointCommand> _validator;
    private readonly IRepository<Endpoint> _repository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CreateEndpointCommandHandler(
        IValidator<CreateEndpointCommand> validator,
        IRepository<Endpoint> repository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _validator = validator;
        _repository = repository;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var existedEndpoint = await _repository.Query(x => x.Name == request.Name).FirstOrDefaultAsync();

        EndpointType endpointTypeEnum = EndpointType.Get;

        if (request.Type != null && Enum.TryParse(request.Type, true, out EndpointType parsedType))
        {
            endpointTypeEnum = parsedType;
        }

        if (existedEndpoint != null)
        {
            throw new AlreadyExistsException("Permmission with that name already exists.");
        }

        var endpoint = new Endpoint
        {
            Name = request.Name,
            Path = request.Name,
            Description = request.Description ?? "Description needed",
            IsDeleted = false,
            DateCreated = SystemDate.Now,
            IsActive = true,
            Type = endpointTypeEnum,
            CreatedBy = _securityContextAccessor.UserId
        };
        await _repository.Store(endpoint);
        await _repository.CommitChanges();

        //var auditLog = new AuditLog
        //{
        //    UserId = _securityContextAccessor.UserId,
        //    Timestamp = SystemDate.Now,
        //    Action = "CREATE",
        //    ObjectId = endpoint.Id,
        //    Object = endpoint.GetType().Name,
        //    Category = "Endpoint",
        //    Log = $"Endpoint Created successfully with ID: {endpoint.Id} by User ID: {_securityContextAccessor.UserId}"
        //};

        //await _auditLogService.LogEventAsync(auditLog);

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission {endpoint.Name} Created",
        };

    }
}
