using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly IValidator<DeleteUserCommand> _validator;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DeleteUserCommandHandler(
        IRepository<User> repository,
        IValidator<DeleteUserCommand> validator,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _validator = validator;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var user = await _repository.Query(x => x.Id == request.UserId).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found");

        user.IsActive = false;
        user.IsDeleted = true;
        user.DateDeleted = SystemDate.Now;

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
