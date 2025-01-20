using Amazon.Runtime.Internal.Util;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<GameService> _logger;

    public GameService(
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<GameService> logger
        )
    {
        _httpClientFactory = httpClientFactory.CreateClient("ApiGateway");
        _hubApiClient = hubApiClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<object> GetAll(FilterGamesDto? filter)
    {
        var response = await _hubApiClient.Get<object>($"{_options.Endpoint}Admin/AllGames?Name={filter.Name}&PromotionId={filter.PromotionId}");

        return response;
    }

    public async Task<bool> GameStatus(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Admin/GameStatus");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<object> ActivateGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Admin/ActivateGame");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> DeactivateGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Admin/DeactivateGame");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurations(string name, int promotionId, int? configurationId)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Admin/Configurations?configurationId={configurationId}&promotionId={promotionId}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ConfigurationsResponse>(jsonString);
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetConfigurationMetadata(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Admin/ConfigurationMetadata");

        if (response.IsSuccessStatusCode)
        {
            var outerJson = await response.Content.ReadAsStringAsync();
            var outerObject = JsonSerializer.Deserialize<ConfigurationMetadataResponse>(outerJson);
            return outerObject;
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> GetGame(string name)
    {
        var response = await _httpClientFactory.GetAsync($"/{Uri.EscapeDataString(name)}Api/Hub/GameShortInfo");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GameShortInfo>(jsonString);
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> CreateConfiguration(string name, object configurationJson)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "The name parameter cannot be null or empty.");
        }

        try
        {
            var jsonString = configurationJson.ToString();
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PostAsync($"/{Uri.EscapeDataString(name)}Api/Admin/CreateConfiguration", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request to CreateConfiguration failed with status {response.StatusCode}. Response: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred in CreateConfiguration: {ex.Message}");
            throw;
        }
    }

    public async Task<object> UpdateConfiguration(string name, object configurationJson)
    {
        if (name != null)
        {
            var jsonContent = JsonSerializer.Serialize(configurationJson);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClientFactory.PutAsJsonAsync<object>($"/{Uri.EscapeDataString(name)}Api/Admin/CreateConfiguration", content);

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

        var response = await _httpClientFactory.PatchAsync($"/{Uri.EscapeDataString(name)}Api/Admin/ActivateConfiguration?id={id}", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
    }

    public async Task<object> DeactivateConfiguration(string name, int id)
    {
        try
        {
            var url = $"/{Uri.EscapeDataString(name)}Api/Admin/DeactivateConfiguration?id={id}";

            var response = await _httpClientFactory.PatchAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException($"Failed to retrieve data: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeactivateConfiguration: {ex.Message}");
            throw;
        }
    }
}