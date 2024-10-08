using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

public class ActivateAccountCommandHandler : BaseCommandHandler<ActivateAccountCommand, ApplicationResult>
{
    private readonly IRepository<User> _repository;

    public ActivateAccountCommandHandler(
        CommandContext<ActivateAccountCommand> context,
        IRepository<User> repository
        ) : base( context )
    {
        _repository = repository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var user = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

        if (user == null || user.VerificationCode != request.Code)
            throw new BadRequestException("Invalid activation code.");

        if (user.VerificationCodeExpiration < DateTime.UtcNow)
            throw new BadRequestException("Activation code has expired.");

        user.IsActive = true;
        user.IsVerified = true;
        user.VerificationPurpose = Shared.Enums.VerificationPurpose.AccountActivation;
        user.VerificationCode = null;
        user.VerificationCodeExpiration = null;

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true, Data = "Account activated successfully." };
    }
}
