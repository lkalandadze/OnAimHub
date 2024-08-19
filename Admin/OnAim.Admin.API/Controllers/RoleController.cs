using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.Role.Create;
using OnAim.Admin.APP.Commands.Role.Update;
using OnAim.Admin.APP.Queries.Role.GetAll;
using OnAim.Admin.APP.Queries.Role.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    public class RoleController : ApiControllerBase
    {
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CreateRoleCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand roleModel)
           => Ok(Mediator.Send(roleModel));

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
            => Ok(Mediator.Send(new GetAllRolesQuery()));

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
            => Ok(await Mediator.Send(new GetRoleByIdQuery(id)));

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoleRequest model)
            => Ok(await Mediator.Send(new UpdateRoleCommand(id, model)));
    }
}
