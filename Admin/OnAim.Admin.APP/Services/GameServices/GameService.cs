using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Contracts.Dtos.Game;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OnAim.Admin.APP.Services.Game;

public class GameService : IGameService
{
    private readonly HttpClient _httpClientFactory;
    private readonly HubApiClientOptions _options;
    private readonly IHubApiClient _hubApiClient;

    public GameService(
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        IHttpClientFactory httpClientFactory
        )
    {
        _httpClientFactory = httpClientFactory.CreateClient("ApiGateway");
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    public async Task<object> GetAll(FilterGamesDto? filter)
    {
        var response = await _hubApiClient.Get<object>($"{_options.Endpoint}Admin/AllGames?Name={filter.Name}&PromotionId={filter.PromotionId}");

        return response;
    }

    public async Task<bool> GameStatus(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}+Api/Admin/GameStatus");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<object> ActivateGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{name}/Admin/ActivateGame");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> DeactivateGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{name}/Admin/DeactivateGame");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfiguration(string name, int id)
    {
        var response = await _httpClientFactory.GetAsync($"/{name}/Admin/ConfigurationById?id={id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurations(string name, int promotionId)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}+Api/Admin/Configurations?promotionId={promotionId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurationMetadata(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}+Api/Admin/ConfigurationMetadata");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<string> GetGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}+Api/Hub/GameShortInfo");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> CreateConfiguration(string name, GameConfigurationDto configurationJson)
    {
        if (name != null)
        {
            var jsonContent = JsonSerializer.Serialize(configurationJson);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PostAsJsonAsync<object>($"/{Uri.EscapeDataString(name)}+Api/Admin/CreateConfiguration", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        throw new HttpRequestException($"Failed to retrieve data");
    }

    public async Task<object> UpdateConfiguration(string name, GameConfigurationDto configurationJson)
    {
        if (name != null)
        {
            var jsonContent = JsonSerializer.Serialize(configurationJson);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PutAsJsonAsync<object>($"/{Uri.EscapeDataString(name)}+Api/Admin/CreateConfiguration", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        throw new HttpRequestException($"Failed to retrieve data");
    }

    public async Task<object> ActivateConfiguration(string name, int id)
    {
        using var content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PatchAsync($"/{Uri.EscapeDataString(name)}+Api/Admin/ActivateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> DeactivateConfiguration(string name, int id)
    {
        using var content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PatchAsync($"/{Uri.EscapeDataString(name)}+Api/Admin/DeactivateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }
}
