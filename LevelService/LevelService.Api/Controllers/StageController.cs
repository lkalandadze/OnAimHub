using LevelService.Application.Features.StageFeatures.Commands.Create;
using LevelService.Application.Features.StageFeatures.Commands.Delete;
using LevelService.Application.Features.StageFeatures.Commands.Update;
using LevelService.Application.Features.StageFeatures.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class StageController : BaseApiController
{
    public StageController()
    {
        
    }

    [HttpPost(nameof(CreateStage))]
    public async Task CreateStage([FromBody] CreateStageCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(DeleteStage))]
    public async Task DeleteStage([FromBody] DeleteStageCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateStage))]
    public async Task UpdateStage([FromBody] UpdateStageCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetStages))]
    public async Task<GetStageQueryResponse> GetStages([FromQuery] GetStageQuery request) => await Mediator.Send(request);
}
