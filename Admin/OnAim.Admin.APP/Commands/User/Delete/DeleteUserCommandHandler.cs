using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.User.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IValidator<DeleteUserCommand> _validator;

        public DeleteUserCommandHandler(
            IRepository<Infrasturcture.Entities.User> repository,
            IValidator<DeleteUserCommand> validator
            )
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _repository.Query(x => x.Id == request.UserId).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsActive = false;
                user.DateDeleted = SystemDate.Now;
                await _repository.CommitChanges();
            }
        }
    }
}
