using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;
    private readonly IValidator<DeleteEndpointCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DeleteEndpointCommandHandler(
        IRepository<Endpoint> repository,
        IValidator<DeleteEndpointCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var endpoint = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (endpoint == null)
        {
            throw new NotFoundException("Permission Not Found");
        }

        endpoint.IsActive = false;
        endpoint.IsDeleted = true;
        endpoint.DateDeleted = SystemDate.Now;
        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
