using MediatR;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using static OnAim.Admin.APP.Exceptions.Exceptions;

namespace OnAim.Admin.APP.Queries.Role.GetById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository, IEndpointGroupRepository endpointGroupRepository)
        {
            _roleRepository = roleRepository;
            _endpointGroupRepository = endpointGroupRepository;
        }
        public async Task<ApplicationResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetRoleById(request.Id);

            if (role == null)
            {
                throw new RoleNotFoundException("Role Doesn't Exist");
            }

            //var endpointGroups = await _endpointGroupRepository.

            return new ApplicationResult
            {
                Success = true,
                Data = role,
                Errors = null
            };
        }
    }
}
