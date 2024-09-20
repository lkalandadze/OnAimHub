using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.API.Service.Endpoint;
using OnAim.Admin.APP.Commands.Domain.Create;
using OnAim.Admin.APP.Commands.User.Activate;
using OnAim.Admin.APP.Commands.User.ChangePassword;
using OnAim.Admin.APP.Commands.User.Create;
using OnAim.Admin.APP.Commands.User.Delete;
using OnAim.Admin.APP.Commands.User.ForgotPassword;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Commands.User.ProfileUpdate;
using OnAim.Admin.APP.Commands.User.Registration;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.APP.Features;
using OnAim.Admin.APP.Queries.User.GetAllUser;
using OnAim.Admin.APP.Queries.User.GetById;
using OnAim.Admin.APP.Queries.User.GetUserLogs;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Models;
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

        [HttpGet("UserLogs /{id}")]
        public async Task<IActionResult> UserLogs([FromRoute] int id, [FromQuery] AuditLogFilter filter)
            => Ok(await Mediator.Send(new GetUserLogsQuery(id, filter)));

        [HttpGet("GetActionTypes")]
        [AllowAnonymous]
        public IActionResult GetActionTypes()
        {
            return Ok(ActionTypes.All);
        }
        [HttpGet("GetEntityNames")]
        [AllowAnonymous]
        public IActionResult GetEntityNames()
        {
            return Ok(EntityNames.All);
        }

        //[HttpGet("Download")]
        //public async Task<IResult> Download([FromQuery] UsersExportQuery query)
        //{
        //    var result = await Mediator.Send(query);
        //    return result;
        //}

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
            => Ok(await Mediator.Send(new DeleteUserCommand(id)));

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
            => Ok(await Mediator.Send(request));

        [HttpPost("activate")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("ForgotPasswordRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordRequest([FromBody] ForgotPasswordCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ResetPassword command)
            => Ok(await Mediator.Send(command));

    }
}
