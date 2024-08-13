using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;
using OnAim.Admin.APP.Commands.Role.Create;
using OnAim.Admin.APP.Commands.Role.Update;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Queries.Role.GetAll;
using OnAim.Admin.APP.Queries.Role.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [Permission("Role/Create")]
        [ProducesResponseType(typeof(CreateRoleCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand roleModel)
        {
            return Ok(_mediator.Send(roleModel));
        }

        [HttpGet("GetAll")]
        [Permission("Role/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mediator.Send(new GetAllRolesQuery()));
        }

        [Permission("Role/Get")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _mediator.Send(new GetRoleByIdQuery(id)));
        }

        [Permission("Role/Update")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRoleRequest model)
        {
            var command = new UpdateRoleCommand(id, model);
            return Ok(_mediator.Send(command));
        }
    }
}
