using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public class UserProfileUpdateCommandHandler : ICommandHandler<UserProfileUpdateCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly IValidator<UserProfileUpdateCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UserProfileUpdateCommandHandler(
        IRepository<User> repository,
        IValidator<UserProfileUpdateCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var user = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new NotFoundException("User Not Found");

        var userClone = new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            IsActive = user.IsActive,
            UserRoles = user.UserRoles,
        };

        user.FirstName = request.profileUpdateRequest.FirstName;
        user.LastName = request.profileUpdateRequest.LastName;
        user.Phone = request.profileUpdateRequest.Phone;
        user.DateUpdated = SystemDate.Now;

        await _repository.CommitChanges();

        var changeLog = AuditLogger.GenerateChangeLog(userClone, user);

        return new ApplicationResult { Success = true };
    }
}
