using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.User.AssignRole;
using OnAim.Admin.APP.Commands.User.Create;
using OnAim.Admin.APP.Commands.User.Delete;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Commands.User.RemoveRole;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.APP.Models.Request.User;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Queries.Role.GetUserRoles;
using OnAim.Admin.APP.Queries.User.GetAllUser;
using OnAim.Admin.APP.Queries.User.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net;
using System.Security.Claims;

namespace OnAim.Admin.API.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet("GetMe")]
        public async Task<IActionResult> GetMe()
        {
            //var user = _appContext.UserId.Value;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found.");
            }

            //var userId = _appContext.UserId.Value;

            return Ok(new GetUserByIdQuery(Convert.ToInt32(userId)));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilter model)
            => Ok(await Mediator.Send(new GetAllUserQuery(model)));

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
            => Ok(Mediator.Send(new GetUserByIdQuery(id)));

        [HttpGet("GetUserRoles/{id}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] int id)
            => Ok(await Mediator.Send(new GetUserRolesQuery(id)));

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateUserCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ApplicationResult> Register([FromBody] CreateUserCommand model)
            => await Mediator.Send(model);

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<AuthResultDto> Login([FromBody] LoginUserRequest model)
            => await Mediator.Send(new LoginUserCommand(model));

        [HttpPost("AssignRole/{id}")]
        public async Task<IActionResult> AssignRole([FromRoute] int id, [FromBody] int roleId)
            => Ok(await Mediator.Send(new AssignRoleToUserCommand(id, roleId)));

        [HttpPost("RemoveRole/{id}")]
        public async Task<IActionResult> RemoveRole([FromRoute] int id, [FromBody] int roleId)
            => Ok(await Mediator.Send(new RemoveRoleCommand(id, roleId)));

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto model)
            => Ok(await Mediator.Send(new UpdateUserCommand(id, model)));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => Ok(Mediator.Send(new DeleteUserCommand(id)));
    }
}
