using Microsoft.AspNetCore.Mvc;

namespace MissionService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MissionController : BaseApiController
{
    public MissionController()
    {
    }

    [HttpGet(nameof(HealthCheck))]
    public ActionResult HealthCheck()
    {
        return Ok();
    }
}
