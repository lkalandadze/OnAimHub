using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.Domain.Create
{
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

            return new ApplicationResult { Success = true };
        }
    }
}
