using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;

public sealed record GetAllRolesQuery(RoleFilter Filter) : IQuery<ApplicationResult>;
