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

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public class DeleteEmailDomainCommandHandler : ICommandHandler<DeleteEmailDomainCommand, ApplicationResult>
{
    private readonly IRepository<AllowedEmailDomain> _repository;
    private readonly IValidator<DeleteEmailDomainCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DeleteEmailDomainCommandHandler(
        IRepository<AllowedEmailDomain> repository,
        IValidator<DeleteEmailDomainCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(DeleteEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var domain = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (domain == null)
        {
            throw new NotFoundException("Domain Not Found b");
        }

        domain.IsDeleted = true;
        domain.IsActive = false;
        domain.DateDeleted = SystemDate.Now;

        await _repository.CommitChanges();

        //var auditLog = new AuditLog
        //{
        //    UserId = _securityContextAccessor.UserId,
        //    Timestamp = SystemDate.Now,
        //    Action = "DELETE",
        //    ObjectId = domain.Id,
        //    Object = domain.GetType().Name,
        //    Category = "Domain",
        //    Log = $"Domain Deleted successfully with ID: {domain.Id} by User ID: {_securityContextAccessor.UserId}"
        //};

        //await _auditLogService.LogEventAsync(auditLog);

        return new ApplicationResult { Success = true };
    }
}
