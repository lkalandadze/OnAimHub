using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.Application.Features.IdentityFeatures.Commands.RefreshTokens;
using Hub.Application.Services.Abstract;
using Hub.Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<Response<CreateAuthenticationTokenResponse>>> Auth(CreateAuthenticationTokenRequest request) 
                                                => await Mediator.Send(request);

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<Response<RefreshTokensCommandResponse>>> RefreshToken([FromBody] RefreshTokensCommand request)
                                                => await Mediator.Send(request);
}