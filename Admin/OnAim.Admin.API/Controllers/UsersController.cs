using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.User.ChangePassword;
using OnAim.Admin.APP.Commands.User.Create;
using OnAim.Admin.APP.Commands.User.Delete;
using OnAim.Admin.APP.Commands.User.Domain;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Commands.User.ProfileUpdate;
using OnAim.Admin.APP.Commands.User.Registration;
using OnAim.Admin.APP.Commands.User.ResetPassword;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.APP.Models.Request.User;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Queries.User.GetAllUser;
using OnAim.Admin.APP.Queries.User.GetById;
using OnAim.Admin.APP.Queries.User.UsersExport;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await Mediator.Send(new GetUserByIdQuery(Convert.ToInt32(userId))));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilter model)
            => Ok(await Mediator.Send(new GetAllUserQuery(model)));

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
            => Ok(await Mediator.Send(new GetUserByIdQuery(id)));

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] UsersExportQuery query)
        {
            var result = await Mediator.Send(query);
            return result;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(CreateUserCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ApplicationResult> Create([FromBody] CreateUserCommand model)
            => await Mediator.Send(model);

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegistrationCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegistrationCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<AuthResultDto> Login([FromBody] LoginUserRequest model)
            => await Mediator.Send(new LoginUserCommand(model));

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest model)
            => Ok(await Mediator.Send(new UpdateUserCommand(id, model)));

        [HttpPut("ProfileUpdate/{id}")]
        public async Task<IActionResult> ProfileUpdate([FromRoute] int id, [FromBody] ProfileUpdateRequest profile)
            => Ok(await Mediator.Send(new UserProfileUpdateCommand(id, profile)));

        [HttpPost("InsertDomain")]
        public async Task<IActionResult> InsertDomain([FromBody] CreateEmailDomainCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => Ok(Mediator.Send(new DeleteUserCommand(id)));
    }
}
