using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Game;

namespace OnAim.Admin.APP.Services.Game;

public class GameService : IGameService
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public GameService(IHubApiClient hubApiClient,IOptions<HubApiClientOptions> options)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    public async Task<ApplicationResult<List<GameListDto>>> GetAll()
    {
        var result = await _hubApiClient.Get<List<GameListDto>>($"{_options.Endpoint}Admin/Games");

        var games = result.Select(x => new GameListDto
        {
            Id = x.Id,
            Name = x.Name,
            Configurations = x.Configurations,
            Description = x.Description,
            Segments = x.Segments,
            LaunchDate = x.LaunchDate,
            Status = x.Status,
        }).ToList();

        return new ApplicationResult<List<GameListDto>> { Success = true, Data = games };
    }

    public async Task<object> GetGame(int id)
    {
        var result = await _hubApiClient.Get<object>($"{_options.Endpoint}Admin/Games");

        return result;
    }

    public async Task<object> GetConfiguration(int id)
    {
        var result = await _hubApiClient.Get<object>($"{_options.Endpoint}Admin/Games");

        return result;
    }
}
