using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Queries.Player.GetAll;
using OnAim.Admin.APP.Queries.Player.GetById;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.API.Controllers
{
    public class PlayerController : ApiControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PlayerFilter filter)
            => Ok(await Mediator.Send(new GetAllPlayerQuery(filter)));

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
            => Ok(await Mediator.Send(new GetPlayerByIdQuery(id)));
    }
}
