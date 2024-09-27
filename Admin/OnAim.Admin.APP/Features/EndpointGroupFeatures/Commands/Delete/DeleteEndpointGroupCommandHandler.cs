using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandHandler : ICommandHandler<DeleteEndpointGroupCommand, ApplicationResult>
{
    private readonly IRepository<EndpointGroup> _repository;
    private readonly IValidator<DeleteEndpointGroupCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DeleteEndpointGroupCommandHandler(
        IRepository<EndpointGroup> repository,
        IValidator<DeleteEndpointGroupCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var group = await _repository.Query(x => x.Id == request.GroupId).FirstOrDefaultAsync();

        if (group == null)
        {
            throw new NotFoundException("Permission Group Not Found");
        }

        group.IsDeleted = true;
        group.IsActive = false;
        group.DateDeleted = SystemDate.Now;

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
