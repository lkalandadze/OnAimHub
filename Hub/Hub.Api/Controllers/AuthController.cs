using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("Test")]
    public Task<string> Test()
    {
        return Task.FromResult("Test is successful");
    }
}
