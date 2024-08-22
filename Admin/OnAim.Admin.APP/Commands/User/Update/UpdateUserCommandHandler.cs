using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserCommandHandler(
            IUserRepository userRepository, 
            IRoleRepository roleRepository,
            IEndpointGroupRepository endpointGroupRepository,
            IValidator<UpdateUserCommand> validator
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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

            var user = await _userRepository.GetById(request.Id);

            if (user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            user.FirstName = request.Model.FirstName;
            user.LastName = request.Model.LastName;
            user.Phone = request.Model.Phone;


            //if (request.Model.Roles != null && request.Model.Roles.Any())
            //{
            //    var roleIds = request.Model.Roles.Select(r => r.Id).ToList();
            //    var roles = await _roleRepository.GetRolesByIdsAsync(roleIds);

            //    foreach (var role in roles)
            //    {
            //        var userRole = new UserRole
            //        {
            //            UserId = user.Id,
            //            RoleId = role.Id
            //        };
            //        await _roleRepository.AssignRoleToUserAsync(userRole.UserId, role.Id);
            //    }
            //}

            return new ApplicationResult
            {
                Success = true,
                Data = user,
                Errors = null
            };
        }
    }
}
