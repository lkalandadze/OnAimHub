using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Queries.EndPoint.EndpointsExport
{
    public record EndpointsExportQuery(
        EndpointFilter Filter,
        List<int>? EndpointIds,
        List<string>? SelectedColumns
        ) : IQuery<IResult>;
}
