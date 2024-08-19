using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserRolesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ApplicationResult> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _userRepository.GetUserRolesAsync(request.Id);

            return new ApplicationResult
            {
                Success = true,
                Data = roles,
                Errors = null
            };
        }
    }
}
