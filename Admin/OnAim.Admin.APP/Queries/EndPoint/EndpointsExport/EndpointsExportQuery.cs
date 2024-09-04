using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;

namespace OnAim.Admin.APP.Queries.EndPoint.EndpointsExport
{
    public record EndpointsExportQuery(EndpointFilter Filter, List<int> EndpointIds) : IQuery<FileContentResult>;
}
