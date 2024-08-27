using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;
        private readonly IValidator<DeleteRoleCommand> _validator;

        public DeleteRoleCommandHandler(
            IRepository<Infrasturcture.Entities.Role> repository,
            IValidator<DeleteRoleCommand> validator
            )
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _repository.Query(x => x.Id == request.Id)
                .Include(r => r.UserRoles)
                .Include(r => r.RoleEndpointGroups)
                .FirstOrDefaultAsync();

            if (role != null)
            {
                role.IsActive = false;
                role.DateDeleted = SystemDate.Now;
                await _repository.CommitChanges();
            }

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}
