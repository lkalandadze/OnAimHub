using Hub.Application.Models.Auth;
using Hub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<AuthResultModel>> Auth(string token)
    {
        var result = await _authService.AuthAsync(token);

        return !result.Success ? StatusCode(401) : StatusCode(200, result);
    }
}
