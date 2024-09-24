using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Queries.User.GetAllUser
{
    public sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;

        public GetAllUserQueryHandler(IRepository<Infrasturcture.Entities.User> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query(x =>
                            string.IsNullOrEmpty(request.UserFilter.Name) || x.FirstName.Contains(request.UserFilter.Name));

            if(request.UserFilter.IsExisted.HasValue)
                query = query.Where(x => x.IsDeleted == false);

            if (request.UserFilter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.UserFilter.IsActive.Value);

            if (request.UserFilter.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.UserFilter.IsDeleted);

            if (request.UserFilter.RegistrationDateFrom.HasValue)
                query = query.Where(x => x.DateCreated >= request.UserFilter.RegistrationDateFrom.Value);

            if (request.UserFilter.RegistrationDateTo.HasValue)
                query = query.Where(x => x.DateCreated <= request.UserFilter.RegistrationDateTo.Value);

            if (request.UserFilter.LoginDateFrom.HasValue)
                query = query.Where(x => x.LastLogin >= request.UserFilter.LoginDateFrom.Value);

            if (request.UserFilter.LoginDateTo.HasValue)
                query = query.Where(x => x.LastLogin <= request.UserFilter.LoginDateTo.Value);

            if (request.UserFilter.RoleIds?.Any() == true)
                query = query.Where(x => x.UserRoles.Any(ur => request.UserFilter.RoleIds.Contains(ur.RoleId)));

            var paginatedResult = await Paginator.GetPaginatedResult(
                query,
                request.UserFilter,
                user => new UsersModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    IsDeleted = user.IsDeleted,
                    Phone = user.Phone,
                    DateCreated = user.DateCreated,
                    DateUpdated = user.DateUpdated,
                    Roles = user.UserRoles.Select(xx => new RoleDto
                    {
                        Id = xx.RoleId,
                        Name = xx.Role.Name,
                        IsActive = xx.Role.IsActive,
                    }).ToList(),
                },
                cancellationToken
            );

            return new ApplicationResult
            {
                Success = true,
                Data = paginatedResult
            };
        }
    }
}
