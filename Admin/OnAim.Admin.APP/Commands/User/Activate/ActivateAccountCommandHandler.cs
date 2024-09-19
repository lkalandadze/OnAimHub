using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;

namespace OnAim.Admin.APP.Commands.User.Activate
{
    public class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;

        public ActivateAccountCommandHandler(
            IRepository<Infrasturcture.Entities.User> repository
            )
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Query(x => x.Email == request.Email && !x.IsDeleted).FirstOrDefaultAsync();

            if (user == null || user.ActivationCode != request.Code)
            {
                throw new BadRequestException("Invalid activation code.");
            }

            if (user.ActivationCodeExpiration < DateTime.UtcNow)
            {
                throw new BadRequestException("Activation code has expired.");
            }

            user.IsActive = true;
            user.IsVerified = true;
            user.ActivationCode = null;
            user.ActivationCodeExpiration = null;

            await _repository.CommitChanges();

            return new ApplicationResult { Success = true, Data = "Account activated successfully." };
        }
    }
}
