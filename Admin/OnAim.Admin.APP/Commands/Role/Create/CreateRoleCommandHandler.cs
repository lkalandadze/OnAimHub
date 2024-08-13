using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(
            IRoleRepository roleRepository
            )
        {
            _roleRepository = roleRepository;
        }
        public async Task<ApplicationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.CreateRoleAsync(request.request);

            if (role == null)
            {
                throw new Exception("Role Not Created");
            }

            return new ApplicationResult
            {
                Success = true,
                Data = role,
                Errors = null
            };
        }
    }
}
