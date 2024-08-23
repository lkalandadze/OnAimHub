using MediatR;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetAllUser
{
    public sealed class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetAllUser(request.UserFilter);

            var result = res.Items.Select(x => new UsersModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Roles = x.Roles.Select(z => new RoleDto
                {
                    Id = z.Id,
                    Name = z.Name,
                    IsActive = z.IsActive,
                }).ToList()
            }).ToList();

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<UsersModel>
                {
                    PageNumber = res.PageNumber,
                    PageSize = res.PageSize,
                    TotalCount = res.TotalCount,
                    Items = result
                },
                Errors = null
            };
        }
    }
}
