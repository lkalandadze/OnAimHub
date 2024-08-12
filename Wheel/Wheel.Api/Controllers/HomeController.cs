using Microsoft.AspNetCore.Mvc;

namespace Wheel.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet(nameof(HealthCheck))]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}
