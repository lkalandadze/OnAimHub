using MediatR;
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

            return new ApplicationResult
            {
                Success = true,
                Data = res,
                Errors = null
            };
        }
    }
}
