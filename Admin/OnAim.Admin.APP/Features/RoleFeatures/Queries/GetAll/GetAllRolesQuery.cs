using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;

public sealed record GetAllRolesQuery(RoleFilter Filter) : IQuery<ApplicationResult<PaginatedResult<RoleShortResponseModel>>>;
