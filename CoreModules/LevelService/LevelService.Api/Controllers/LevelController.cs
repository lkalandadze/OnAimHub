using LevelService.Application.Features.LevelFeatures.Commands.Create;
using LevelService.Application.Features.LevelFeatures.Queries.Get;
using Microsoft.AspNetCore.Mvc;

namespace LevelService.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class LevelController : BaseApiController
{
    public LevelController()
    {
    }

    [HttpPost(nameof(CreateLevels))]
    public async Task CreateLevels([FromBody] CreateLevelsCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLevels))]
    public async Task<GetLevelsQueryResponse> GetLevels([FromQuery] GetLevelsQuery request) => await Mediator.Send(request);
}
