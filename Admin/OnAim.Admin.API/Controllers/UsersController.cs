using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Delete;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Login;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Update;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;
using OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;
using OnAim.Admin.Contracts.Dtos.User;
using System.Net;
using System.Security.Claims;

namespace OnAim.Admin.API.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet(nameof(GetMe))]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(await Mediator.Send(new GetUserByIdQuery(Convert.ToInt32(userId))));
    }

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter model)
        => Ok(await Mediator.Send(new GetAllUserQuery(model)));

    [HttpGet(nameof(Get) + "/{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetUserByIdQuery(id)));

    [HttpGet(nameof(UserLogs) + "/{id}")]
    public async Task<IActionResult> UserLogs([FromRoute] int id, [FromQuery] AuditLogFilter filter)
        => Ok(await Mediator.Send(new GetUserLogsQuery(id, filter)));

    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(CreateUserCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Contracts.ApplicationInfrastructure.Validation.Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ApplicationResult> Create([FromBody] CreateUserCommand model)
        => await Mediator.Send(model);

    [HttpPost(nameof(Register))]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegistrationCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Contracts.ApplicationInfrastructure.Validation.Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegistrationCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(Login))]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResultDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Contracts.ApplicationInfrastructure.Validation.Error), (int)HttpStatusCode.BadRequest)]
    public async Task<AuthResultDto> Login([FromBody] LoginUserRequest model)
        => await Mediator.Send(new LoginUserCommand(model));

    [HttpPost(nameof(VerifyOtp))]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(ChangePassword))]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(Update) + "/{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest model)
        => Ok(await Mediator.Send(new UpdateUserCommand(id, model)));

    [HttpPut(nameof(ProfileUpdate) + "/{id}")]
    public async Task<IActionResult> ProfileUpdate([FromRoute] int id, [FromBody] ProfileUpdateRequest profile)
        => Ok(await Mediator.Send(new UserProfileUpdateCommand(id, profile)));

    [HttpPost(nameof(Delete))]
    public async Task<IActionResult> Delete([FromBody] List<int> ids)
        => Ok(await Mediator.Send(new DeleteUserCommand(ids)));

    [HttpPost(nameof(RefreshToken))]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
        => Ok(await Mediator.Send(request));

    [HttpPost(nameof(ActivateAccount))]
    [AllowAnonymous]
    public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(ForgotPasswordRequest))]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordRequest([FromBody] ForgotPasswordCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(ForgotPassword))]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ResetPassword command)
        => Ok(await Mediator.Send(command));

}
