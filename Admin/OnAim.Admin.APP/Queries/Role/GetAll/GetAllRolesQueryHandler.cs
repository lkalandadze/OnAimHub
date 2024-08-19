using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = _roleRepository.GetAllRolesAsync().Result;
            return new ApplicationResult
            {
                Success = true,
                Data = roles,
                Errors = null
            };
        }
    }
}
