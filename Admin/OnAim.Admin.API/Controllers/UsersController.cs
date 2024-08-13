using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;
using OnAim.Admin.APP.Commands.User.AssignRole;
using OnAim.Admin.APP.Commands.User.Create;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Models.Request.User;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Queries.Role.GetUserRoles;
using OnAim.Admin.APP.Queries.User.GetAllUser;
using OnAim.Admin.APP.Queries.User.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Permission("Users/GetAll")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilter model)
        {
            var query = new GetAllUserQuery(model);
            return Ok(await _mediator.Send(query));
        }

        [Permission("Users/Get")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(_mediator.Send(new GetUserByIdQuery(id)));
        }

        [Permission("Users/GetUserRoles")]
        [HttpGet("GetUserRoles/{id}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string id)
        {
            return Ok(await _mediator.Send(new GetUserRolesQuery(id)));
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(CreateUserCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ApplicationResult> Register([FromBody] CreateUserCommand model)
        {
            return await _mediator.Send(model);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<AuthResultDto> Login([FromBody] LoginUserRequest model)
        {
            return await _mediator.Send(new LoginUserCommand(model));
        }

        [Permission("Users/AssignRole")]
        [HttpPost("AssignRole/{id}")]
        public async Task<IActionResult> AssignRole([FromRoute] string id, [FromBody] string roleId)
        {
            var command = new AssignRoleToUserCommand(id, roleId);

            return Ok(await _mediator.Send(command));
        }

        [Permission("Users/Update")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserDto model)
        {
            var command = new UpdateUserCommand(id, model);
            return Ok(await _mediator.Send(command));
        }
    }
}
