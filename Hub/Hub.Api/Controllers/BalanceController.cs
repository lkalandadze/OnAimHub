using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BalanceController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(42);
        }
    }
}
