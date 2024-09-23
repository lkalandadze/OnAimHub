using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LeaderboardController : BaseApiController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
