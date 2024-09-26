using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public sealed class UpdateEndpointCommandHandler : ICommandHandler<UpdateEndpointCommand, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;
    private readonly IValidator<UpdateEndpointCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UpdateEndpointCommandHandler(
        IRepository<Endpoint> repository,
        IValidator<UpdateEndpointCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var ep = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (ep == null)
        {
            throw new NotFoundException("Permission Not Found");
        }

        ep.Description = request.Endpoint.Description;
        ep.IsActive = request.Endpoint.IsActive ?? true;
        ep.DateUpdated = SystemDate.Now;

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission {ep.Name} Successfully Updated"
        };
    }
}
