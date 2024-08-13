using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<ApplicationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            if (request.Model.Roles != null && request.Model.Roles.Any())
            {
                var roleIds = request.Model.Roles.Select(r => r.Id).ToList();
                var roles = await _roleRepository.GetRolesByIdsAsync(roleIds);

                foreach (var role in roles)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    await _roleRepository.AssignRoleToUserAsync(userRole.UserId, role.Id);
                }
            }

            user.FirstName = request.Model.FirstName;
            user.LastName = request.Model.LastName;
            user.Phone = request.Model.Phone;

            return new ApplicationResult
            {
                Success = true,
                Data = user,
                Errors = null
            };
        }
    }
}
