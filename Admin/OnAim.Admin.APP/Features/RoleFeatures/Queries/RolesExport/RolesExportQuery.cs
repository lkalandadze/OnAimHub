using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.RolesExport;

public record RolesExportQuery(
    RoleFilter Filter,
    List<int>? RoleIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
