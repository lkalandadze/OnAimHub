using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public class CreateEmailDomainCommandHandler : ICommandHandler<CreateEmailDomainCommand, ApplicationResult>
{
    private readonly IRepository<AllowedEmailDomain> _repository;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IValidator<CreateEmailDomainCommand> _validator;

    public CreateEmailDomainCommandHandler(
        IRepository<AllowedEmailDomain> repository,
        ISecurityContextAccessor securityContextAccessor,
        IValidator<CreateEmailDomainCommand> validator
        )
    {
        _repository = repository;
        _securityContextAccessor = securityContextAccessor;
        _validator = validator;
    }
    public async Task<ApplicationResult> Handle(CreateEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        if (request.Id != 0)
        {
            var domainn = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            domainn.Domain = request.Domain;
            domainn.DateUpdated = SystemDate.Now;

            await _repository.CommitChanges();
        }

        var existed = await _repository.Query(x => x.Domain == request.Domain).FirstOrDefaultAsync();

        if (existed.IsDeleted)
        {
            existed.IsDeleted = false;
            existed.IsDeleted = true;
            existed.DateUpdated = SystemDate.Now;
            await _repository.CommitChanges();

            return new ApplicationResult
            {
                Success = true,
            };
        }

        if (existed != null)
        {
            throw new AlreadyExistsException("Domain Already Exists");
        }

        var domain = new AllowedEmailDomain
        {
            Domain = request.Domain,
            DateCreated = SystemDate.Now,
            IsActive = true,
            CreatedBy = _securityContextAccessor.UserId
        };

        await _repository.Store(domain);
        await _repository.CommitChanges();

        //var auditLog = new AuditLog
        //{
        //    UserId = _securityContextAccessor.UserId,
        //    Timestamp = SystemDate.Now,
        //    Action = "CREATE",
        //    ObjectId = domain.Id,
        //    Object = domain.GetType().Name,
        //    Category = "Domain",
        //    Log = $"Domain Created successfully with ID: {domain.Id} by User ID: {_securityContextAccessor.UserId}"
        //};

        //await _auditLogService.LogEventAsync(auditLog);

        return new ApplicationResult { Success = true };
    }
}
