using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.RolesExport;

public record RolesExportQuery(
    RoleFilter Filter,
    List<int>? RoleIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
