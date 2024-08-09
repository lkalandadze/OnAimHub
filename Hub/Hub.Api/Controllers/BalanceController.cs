using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(42);
        }
    }
}