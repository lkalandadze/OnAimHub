using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;
using OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Paging;
using System.Net;

namespace OnAim.Admin.API.Controllers;

public class RoleController : ApiControllerBase
{
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(CreateRoleCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Contracts.ApplicationInfrastructure.Validation.Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand roleModel)
       => Ok(await Mediator.Send(roleModel));

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<RoleShortResponseModel>>>> GetAll([FromQuery] RoleFilter filter)
        => Ok(await Mediator.Send(new GetAllRolesQuery(filter)));

    [HttpGet(nameof(Get) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<RoleResponseModel>>> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetRoleByIdQuery(id)));

    [HttpPut(nameof(Update) + "/{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoleRequest model)
        => Ok(await Mediator.Send(new UpdateRoleCommand(id, model)));

    [HttpPost(nameof(Delete))]
    public async Task<IActionResult> Delete([FromBody] List<int> ids)
        => Ok(await Mediator.Send(new DeleteRoleCommand(ids)));
}
