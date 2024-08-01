using Microsoft.AspNetCore.Mvc;

namespace ApiGateway
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        [HttpGet("Spin")]
        public int Spin()
        {
            return 37;
        }

        [HttpGet("GameName")]
        public string GameName()
        {
            return "DSADSA";
        }
    }
}
