using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;

namespace OnAim.Admin.APP.Queries.EndpointGroup.EndpointGroupsExport
{
    public record EndpointGroupsExportQuery(EndpointGroupFilter Filter, List<int> GroupIds) : IQuery<FileContentResult>;
}
