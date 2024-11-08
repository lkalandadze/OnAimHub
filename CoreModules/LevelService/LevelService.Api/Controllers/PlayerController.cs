using LevelService.Application.Features.LevelFeatures.Commands.Create;
using LevelService.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LevelService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerController : BaseApiController
{
    private readonly IPlayerService _playerService;
    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost(nameof(ExperienceCheck))]
    public async Task ExperienceCheck([FromBody] TestModel request)
    {
        await _playerService.GrantExperienceAndRewardsAsync(request.PlayerId, request.CurrencyId, request.Amount);
    }
}

public class TestModel
{
    public int PlayerId { get; set; }
    public string CurrencyId { get; set; }
    public int Amount { get; set; }
}