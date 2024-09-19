using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Queries.EndpointGroup.EndpointGroupsExport
{
    public record EndpointGroupsExportQuery(
        EndpointGroupFilter Filter,
        List<int>? GroupIds,
        List<string>? SelectedColumns
        ) : IQuery<IResult>;
}
