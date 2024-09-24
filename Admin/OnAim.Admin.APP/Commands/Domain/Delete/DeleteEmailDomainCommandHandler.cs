using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.Domain.Delete
{
    public class DeleteEmailDomainCommandHandler : ICommandHandler<DeleteEmailDomainCommand, ApplicationResult>
    {
        private readonly IRepository<AllowedEmailDomain> _repository;
        private readonly IValidator<DeleteEmailDomainCommand> _validator;

        public DeleteEmailDomainCommandHandler(IRepository<AllowedEmailDomain> repository, IValidator<DeleteEmailDomainCommand> validator)
        {
            _repository = repository;
            _validator = validator;
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

            return new ApplicationResult { Success = true };
        }
    }
}
