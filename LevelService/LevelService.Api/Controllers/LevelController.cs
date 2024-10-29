using Microsoft.AspNetCore.Mvc;

namespace LevelService.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class LevelController : BaseApiController
{
    public LevelController()
    {
    }

    [HttpGet(nameof(HealthCheck))]
    public ActionResult HealthCheck()
    {
        return Ok();
    }
}
