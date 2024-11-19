using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Contracts.Dtos.Game;
using System.Text;
using System.Text.Json;

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

    public async Task<object> CreateConfiguration(string configurationJson)
    {
        var payload = new
        {
            configurationJson = configurationJson
        };

        var jsonContent = JsonSerializer.Serialize(payload);
        using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PostAsync("/WheelApi/Admin/CreateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> UpdateConfiguration(string configurationJson)
    {
        var payload = new
        {
            configurationJson = configurationJson
        };

        var jsonContent = JsonSerializer.Serialize(payload);
        using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClientFactory.PutAsync("/WheelApi/Admin/CreateConfiguration", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
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
public class CreatePrizeTypeDto
{
    public string Name { get; set; }
    public bool IsMultiplied { get; set; }
    public string CurrencyId { get; set; }
}