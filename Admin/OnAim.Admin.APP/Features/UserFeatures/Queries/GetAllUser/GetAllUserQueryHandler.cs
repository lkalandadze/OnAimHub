using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.User> _repository;

    public GetAllUserQueryHandler(IRepository<Domain.Entities.User> repository)
    {
        _repository = repository;
    }
    public async Task<ApplicationResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query(x =>
            string.IsNullOrEmpty(request.UserFilter.Name) ||
            x.FirstName.ToLower().Contains(request.UserFilter.Name.ToLower()));

        if (request.UserFilter?.HistoryStatus.HasValue == true)
        {
            switch (request.UserFilter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    query = query.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    query = query.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }
    
        if (request.UserFilter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.UserFilter.IsActive.Value);

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
            new List<string> { "Id", "FirstName", "LastName"},
            cancellationToken
        );

        return new ApplicationResult
        {
            Success = true,
            Data = paginatedResult
        };
    }
}
