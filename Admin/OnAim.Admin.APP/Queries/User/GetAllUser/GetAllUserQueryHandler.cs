using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Paging;
using System.Linq.Expressions;

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

            if (request.UserFilter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.UserFilter.IsActive.Value);

            if (request.UserFilter.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.UserFilter.IsDeleted);
            
            if (!request.UserFilter.IsActive.HasValue && !request.UserFilter.IsDeleted.HasValue)
            {
                
            }

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


            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.UserFilter.PageNumber ?? 1;
            var pageSize = request.UserFilter.PageSize ?? 25;

            var sortDescending = request.UserFilter.SortDescending.GetValueOrDefault();
            var sortBy = request.UserFilter.SortBy?.ToLower();

            Expression<Func<Infrasturcture.Entities.User, object>> sortExpression = sortBy switch
            {
                "id" => x => x.Id,
                "name" => x => x.FirstName,
                "lastname" => x => x.LastName,
                _ => x => x.Id
            };

            query = sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);


            var res = query
                .Select(x => new UsersModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    Phone = x.Phone,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Roles = x.UserRoles.Select(xx => new RoleDto
                    {
                        Id = xx.RoleId,
                        Name = xx.Role.Name,
                        IsActive = xx.Role.IsActive,
                    }).ToList(),
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<UsersModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await res.ToListAsync()
                },
            };
        }
    }
}
