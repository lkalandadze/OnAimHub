using Hub.Application.Features.GameFeatures.Queries.GetAllGame;
using Hub.Application.Features.IdentityFeatures.Commands.ApplyPromoCode;
using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;
using Hub.Application.Features.LevelFeatures.Commands.Create;
using Hub.Application.Features.LevelFeatures.Commands.Update;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerProgress;
using Hub.Application.Features.PlayerFeatures.Queries.GetPromoCode;
using Hub.Application.Models.Game;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Wrappers;

namespace Hub.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiExplorerSettings(GroupName = "hub")]
public class HubController : BaseApiController 
{
    #region Authentification

    [AllowAnonymous]
    [HttpPost(nameof(Authentificate))]
    public async Task<ActionResult<Response<CreateAuthenticationTokenResponse>>> Authentificate(CreateAuthenticationTokenCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [AllowAnonymous]
    [HttpPost(nameof(RefreshAuthentification))]
    public async Task<ActionResult<Response<RefreshTokensCommandResponse>>> RefreshAuthentification([FromBody] RefreshTokensCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    #endregion

    #region Games

    [HttpGet("Games")]
    public async Task<ActionResult<IEnumerable<ActiveGameModel>>> Games()
    {
        return Ok(await Mediator.Send(new GetAllGameQuery()));
    }

    #endregion

    #region Players

    [HttpGet(nameof(PlayerBalances))]
    public async Task<ActionResult<GetPlayerBalanceResponse>> PlayerBalances()
    {
        return Ok(await Mediator.Send(new GetPlayerBalanceQuery()));
    }

    [HttpGet(nameof(PlayerProgresses))]
    public async Task<ActionResult<GetPlayerProgressResponse>> PlayerProgresses()
    {
        return Ok(await Mediator.Send(new GetPlayerProgressQuery()));
    }

    [HttpPost(nameof(ApplyPromoCode))]
    public async Task<Unit> ApplyPromoCode(ApplyPromoCodeCommand request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet(nameof(GetPromoCode))]
    public async Task<string> GetPromoCode([FromQuery] GetPromoCodeQuery request)
    {
        return await Mediator.Send(request);
    }

    #endregion

    [AllowAnonymous]
    [HttpPost(nameof(CreateLevels))]
    public async Task<ActionResult<Unit>> CreateLevels([FromBody] CreateLevelCommand request) => await Mediator.Send(request);

    [AllowAnonymous]
    [HttpPut(nameof(UpdateLevels))]
    public async Task<ActionResult<Unit>> UpdateLevels([FromBody] UpdateLevelCommand request) => await Mediator.Send(request);

}