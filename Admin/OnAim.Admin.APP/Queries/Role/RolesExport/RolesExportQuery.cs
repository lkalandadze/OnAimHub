using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Queries.Role.RolesExport
{
    public record RolesExportQuery(
        RoleFilter Filter,
        List<int>? RoleIds,
        List<string>? SelectedColumns
        ) : IQuery<IResult>;
}
