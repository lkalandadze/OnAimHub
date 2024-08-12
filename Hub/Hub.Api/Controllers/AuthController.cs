using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Route("hubapi/[controller]")]
public class AuthController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<CreateAuthenticationTokenResponse>> Auth(CreateAuthenticationTokenRequest request)
    {
        var result = await Mediator.Send(request);

        return !result.Success ? StatusCode(401) : StatusCode(200, result);
    }
}
