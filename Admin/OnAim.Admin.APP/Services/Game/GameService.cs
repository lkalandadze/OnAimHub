using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Shared.DTOs.Game;

namespace OnAim.Admin.APP.Services.Game;

public class GameService : IGameService
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HttpClient _httpClientFactory;
    private readonly HubApiClientOptions _options;

    public GameService(IHubApiClient hubApiClient,IOptions<HubApiClientOptions> options, IHttpClientFactory httpClientFactory)
    {
        _hubApiClient = hubApiClient;
        _httpClientFactory = httpClientFactory.CreateClient("ApiGateway");
        _options = options.Value;
    }

    public async Task<List<GameListDtoItem>> GetAll()
    {
        var response = await _hubApiClient.Get<GameListDto>($"{_options.Endpoint}Admin/AllGames");

        return response.Data;
    }

    public async Task<object> GetConfiguration(int id)
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Hub/ConfigurationById?id={id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurations()
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Hub/Configurations");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<string> GetGame()
    {
        var response = await _httpClientFactory.GetAsync("/WheelApi/Hub/GameShortInfo");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }
}
