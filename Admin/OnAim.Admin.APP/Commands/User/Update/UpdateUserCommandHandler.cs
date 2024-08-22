using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserCommandHandler(
            IUserRepository userRepository, 
            IEndpointGroupRepository endpointGroupRepository,
            IValidator<UpdateUserCommand> validator
            )
        {
            _userRepository = userRepository;
            _endpointGroupRepository = endpointGroupRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _userRepository.UpdateUser(request.Id, request.Model);

            return new ApplicationResult
            {
                Success = true,
                Errors = null
            };
        }
    }
}
