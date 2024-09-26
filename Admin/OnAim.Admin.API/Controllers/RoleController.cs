using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;
using OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;
using OnAim.Admin.Shared.ApplicationInfrastructure.Validation;
using OnAim.Admin.Shared.DTOs.Role;
using System.Net;

namespace OnAim.Admin.API.Controllers;

public class RoleController : ApiControllerBase
{
    [HttpPost("Create")]
    [ProducesResponseType(typeof(CreateRoleCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand roleModel)
       => Ok(await Mediator.Send(roleModel));

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] RoleFilter filter)
        => Ok(await Mediator.Send(new GetAllRolesQuery(filter)));

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetRoleByIdQuery(id)));

    //[HttpGet("Download")]
    //public async Task<IResult> Download([FromQuery] RolesExportQuery query)
    //{
    //    var result = await Mediator.Send(query);
    //    return result;
    //}

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoleRequest model)
        => Ok(await Mediator.Send(new UpdateRoleCommand(id, model)));

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => Ok(await Mediator.Send(new DeleteRoleCommand(id)));
}
