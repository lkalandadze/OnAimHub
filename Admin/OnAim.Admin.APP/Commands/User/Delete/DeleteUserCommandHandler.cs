using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.User.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<DeleteUserCommand> _validator;

        public DeleteUserCommandHandler(
            IUserRepository userRepository, 
            IValidator<DeleteUserCommand> validator
            )
        {
            _userRepository = userRepository;
            _validator = validator;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _userRepository.DeleteUser(request.UserId);
        }
    }
}
