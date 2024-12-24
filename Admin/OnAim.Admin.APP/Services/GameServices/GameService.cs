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

    public async Task<string> GetAll(FilterGamesDto? filter)
    {
        var response = await _hubApiClient.Get<string>($"{_options.Endpoint}Admin/AllGames?Name={filter.Name}&PromotionId={filter.PromotionId}");

        return response;
    }

    public async Task<object> GetConfiguration(int id)
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Admin/ConfigurationById?id={id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurations()
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Admin/Configurations");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurationMetadata()
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Admin/ConfigurationMetadata");

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

    public async Task<object> CreateConfiguration(string gameName, GameConfigurationDto configurationJson)
    {
        if (gameName != null)
        {
            var jsonContent = JsonSerializer.Serialize(configurationJson);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PostAsJsonAsync<object>("/WheelApi/Admin/CreateConfiguration", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        throw new HttpRequestException($"Failed to retrieve data");
    }

    public async Task<object> UpdateConfiguration(string gameName, GameConfigurationDto configurationJson)
    {
        if (gameName != null)
        {
            var jsonContent = JsonSerializer.Serialize(configurationJson);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PutAsJsonAsync<object>("/WheelApi/Admin/CreateConfiguration", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        throw new HttpRequestException($"Failed to retrieve data");
    }

    public async Task<object> ActivateConfiguration(int id)
    {
        using var content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PatchAsync("/WheelApi/Admin/ActivateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> DeactivateConfiguration(int id)
    {
        using var content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PatchAsync("/WheelApi/Admin/DeactivateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetPrizeTypes()
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Admin/PrizeTypes");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetPrizeTypeById(int id)
    {
        var response = await _httpClientFactory.GetAsync($"/WheelApi/Admin/PrizeTypeById?id={id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> CreatePrizeType(CreatePrizeTypeDto createPrize)
    {
        if (createPrize == null)
        {
            throw new ArgumentNullException(nameof(createPrize), "CreatePrizeTypeDto cannot be null.");
        }

        var jsonContent = new StringContent(JsonSerializer.Serialize(createPrize), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PostAsync("/WheelApi/Admin/CreatePrizeType", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> UpdatePrizeType(int id, CreatePrizeTypeDto typeDto)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(typeDto), Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PostAsync($"/WheelApi/Admin/UpdatePrizeType?id={id}", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

}
