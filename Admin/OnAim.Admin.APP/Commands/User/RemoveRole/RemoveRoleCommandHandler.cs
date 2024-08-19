using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.RemoveRole
{
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;

        public RemoveRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<ApplicationResult> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleRepository.RemoveRoleFromUserAsync(request.UserId, request.RoleId);

            return new ApplicationResult
            {
                Success = true,
                Data = null,
                Errors = null
            };
        }
    }
}
