using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

public class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public ActivateAccountCommandHandler(
        IRepository<User> repository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _securityContextAccessor = securityContextAccessor;
    }
    public async Task<ApplicationResult> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

        if (user == null || user.ActivationCode != request.Code)
            throw new BadRequestException("Invalid activation code.");

        if (user.ActivationCodeExpiration < DateTime.UtcNow)
            throw new BadRequestException("Activation code has expired.");

        user.IsActive = true;
        user.IsVerified = true;
        user.ActivationCode = null;
        user.ActivationCodeExpiration = null;

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true, Data = "Account activated successfully." };
    }
}
