using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetById
{
    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ApplicationResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetById(request.Id).Result;

            return new ApplicationResult
            {
                Success = true,
                Data = user,
                Errors = null
            };
        }
    }
}
